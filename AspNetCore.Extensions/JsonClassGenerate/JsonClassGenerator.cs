namespace AspNetCore.Extensions.JsonClassGenerate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonClassGenerator : IJsonClassGeneratorConfig
    {
        public string Example { get; set; }

        public string TargetFolder { get; set; }

        public string Namespace { get; set; }

        public string SecondaryNamespace { get; set; }

        public bool UseProperties { get; set; }

        public bool InternalVisibility { get; set; }

        public bool ExplicitDeserialization { get; set; }

        public bool NoHelperClass { get; set; }

        public string MainClass { get; set; }

        public bool SortMemberFields { get; set; }

        public bool UsePascalCase { get; set; }

        public bool UseNestedClasses { get; set; }

        public bool ApplyObfuscationAttributes { get; set; }

        public bool SingleFile { get; set; }

        public ICodeWriter CodeWriter { get; set; }

        public TextWriter OutputStream { get; set; }

        public bool AlwaysUseNullableValues { get; set; }

        public bool ExamplesInDocumentation { get; set; }

        public bool DeduplicateClasses { get; set; }

        public Action<string> FeedBack { get; set; }

        private bool used = false;

        public bool UseNamespaces { get { return this.Namespace != null; } }

        public string GenerateClasses()
        {
            if (this.CodeWriter == null)
            {
                this.CodeWriter = new CSharpCodeWriter();
            }

            if (this.ExplicitDeserialization && !(this.CodeWriter is CSharpCodeWriter)) throw new ArgumentException("Explicit deserialization is obsolete and is only supported by the C# provider.");

            if (this.used)
            {
                throw new InvalidOperationException("This instance of JsonClassGenerator has already been used. Please create a new instance.");
            }

            this.used = true;

            JObject[] examples;
            var example = this.Example.StartsWith("HTTP/") ? this.Example.Substring(this.Example.IndexOf("\r\n\r\n")) : this.Example;
            using (var sr = new StringReader(example))
            using (var reader = new JsonTextReader(sr))
            {
                var json = JToken.ReadFrom(reader);
                if (json is JArray)
                {
                    examples = ((JArray)json).Cast<JObject>().ToArray();
                }
                else if (json is JObject)
                {
                    examples = new[] { (JObject)json };
                }
                else
                {
                    throw new Exception("Sample JSON must be either a JSON array, or a JSON object.");
                }
            }

            this.Types = new List<JsonType>();
            this.Names.Add(this.MainClass);
            var rootType = new JsonType(this, examples[0])
            {
                IsRoot = true,
            };
            rootType.AssignName(this.MainClass, this.MainClass);
            this.GenerateClass(examples, rootType);

            if (this.DeduplicateClasses)
            {
                this.FeedBack?.Invoke("De-duplicating classes");
                this.DeDuplicateClasses();
            }

            this.FeedBack?.Invoke("Writing classes to disk.");

            this.WriteClassesToFile(this.OutputStream, this.Types);
            return this.OutputStream.ToString();
        }

        private void DeDuplicateClasses()
        {
            // Get the first occurrence classes (original name = assigned name) as we always want these
            var newTypes = (from tt in this.Types
                            where string.Compare(tt.OriginalName, tt.AssignedName, StringComparison.InvariantCultureIgnoreCase) == 0
                            select tt).ToList();

            // If we replace references to classes (Say "Wombats2" with "Womnbats", we need to know it has
            // happen4ed and we need to fix the fields. This is the list of translations.
            var typeNameReplacements = new Dictionary<string, string>();

            // Get the potential duplicate classes. These are classes where the class name does not match
            // the original name (i.e. we added a count to it).
            var possibleDuplicates = from tt in this.Types
                                     where string.Compare(tt.OriginalName, tt.AssignedName, StringComparison.InvariantCultureIgnoreCase) != 0
                                     select tt;

            try
            {
                foreach (var duplicate in possibleDuplicates)
                {
                    var original = newTypes.FirstOrDefault(tt => tt.OriginalName == duplicate.OriginalName);

                    if (this.FirstOccurrenceClassNotFound(original))
                    {
                        newTypes.Add(duplicate);
                        continue;
                    }

                    // Classes are the same - Merge the fields
                    this.MergeFieldFromDuplicateToOriginal(original, duplicate);

                    // Two objects are the 'same', so we want to replace the duplicate with the original. We will
                    // need to fix-up the field types when we are done.
                    typeNameReplacements.Add(duplicate.AssignedName, original.AssignedName);
                }

                // We now need to apply our class name translations to the new base types list. So, something that
                // might currently be referring to Wombats2 wil be changed to refer to Wombats.
                foreach (var jsonType in newTypes)
                {
                    foreach (var field in jsonType.Fields)
                    {
                        var internalTypeName = this.GetInternalTypeName(field);
                        if (internalTypeName != null && typeNameReplacements.ContainsKey(internalTypeName))
                        {
                            field.Type.InternalType.AssignName(typeNameReplacements[internalTypeName], typeNameReplacements[internalTypeName]);
                        }

                        var typeName = this.GetTypeName(field);
                        if (typeName != null && typeNameReplacements.ContainsKey(typeName))
                        {
                            field.Type.AssignName(typeNameReplacements[typeName], typeNameReplacements[typeName]);
                        }
                    }
                }

                // Replace the previous type list with the new type list
                this.Types.Clear();
                newTypes.ForEach(tt => this.Types.Add(tt));
            }
            catch (Exception ex)
            {
                // Worst case scenario - deduplication failed, so generate all the classes.
                Debug.Print($"Deduplication failed:\r\n\n{ex.Message}\r\n\r\n{ex.StackTrace}");
            }
        }

        private void MergeFieldFromDuplicateToOriginal(JsonType original, JsonType duplicate)
        {
            var fieldDifferences = this.GetFieldDifferences(original.Fields, duplicate.Fields, x => x.MemberName);
            foreach (var fieldDifference in fieldDifferences)
            {
                original.Fields.Add(duplicate.Fields.First(fld => fld.MemberName == fieldDifference));
            }
        }

        private string GetInternalTypeName(FieldInfo field)
        {
            // Sorry about this, but we can get nulls at all sorts of levels. Quite irritating really. So we have to
            // check all the way down to get the assigned name. Returns blank if we fail at any point.
            return field?.Type?.InternalType?.AssignedName;
        }

        private string GetTypeName(FieldInfo field)
        {
            // Sorry about this, but we can get nulls at all sorts of levels. Quite irritating really. So we have to
            // check all the way down to get the assigned name. Returns blank if we fail at any point.
            return field?.Type?.AssignedName;
        }

        private bool FirstOccurrenceClassNotFound(JsonType original) { return original == null; }

        public IEnumerable<string> GetFieldDifferences(IEnumerable<FieldInfo> source, IEnumerable<FieldInfo> other, Func<FieldInfo, string> keySelector)
        {
            var setSource = new HashSet<string>(source.Select(keySelector));
            var setOther = new HashSet<string>(other.Select(keySelector));

            return setOther.Except(setSource);
        }

        private void WriteClassesToFile(string path, IEnumerable<JsonType> types)
        {
            this.FeedBack?.Invoke($"Writing {path}");
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                this.WriteClassesToFile(sw, types);
            }
        }

        private void WriteClassesToFile(TextWriter sw, IEnumerable<JsonType> types)
        {
            var inNamespace = false;
            var rootNamespace = false;

            this.CodeWriter.WriteFileStart(this, sw);
            foreach (var type in types)
            {
                if (this.UseNamespaces && inNamespace && rootNamespace != type.IsRoot && this.SecondaryNamespace != null) { this.CodeWriter.WriteNamespaceEnd(this, sw, rootNamespace);
                    inNamespace = false; }
                if (this.UseNamespaces && !inNamespace) { this.CodeWriter.WriteNamespaceStart(this, sw, type.IsRoot);
                    inNamespace = true;
                    rootNamespace = type.IsRoot; }
                this.CodeWriter.WriteClass(this, sw, type, types.Last() != type);
            }

            if (this.UseNamespaces && inNamespace) this.CodeWriter.WriteNamespaceEnd(this, sw, rootNamespace);
            this.CodeWriter.WriteFileEnd(this, sw);
        }

        private void GenerateClass(JObject[] examples, JsonType type)
        {
            var jsonFields = new Dictionary<string, JsonType>();
            var fieldExamples = new Dictionary<string, IList<object>>();

            var first = true;

            foreach (var obj in examples)
            {
                foreach (var prop in obj.Properties())
                {
                    JsonType fieldType;
                    var currentType = new JsonType(this, prop.Value);
                    var propName = prop.Name;
                    if (jsonFields.TryGetValue(propName, out fieldType))
                    {
                        var commonType = fieldType.GetCommonType(currentType);

                        jsonFields[propName] = commonType;
                    }
                    else
                    {
                        var commonType = currentType;
                        if (first) commonType = commonType.MaybeMakeNullable(this);
                        else commonType = commonType.GetCommonType(JsonType.GetNull(this));
                        jsonFields.Add(propName, commonType);
                        fieldExamples[propName] = new List<object>();
                    }

                    var fe = fieldExamples[propName];
                    var val = prop.Value;
                    if (val.Type == JTokenType.Null || val.Type == JTokenType.Undefined)
                    {
                        if (!fe.Contains(null))
                        {
                            fe.Insert(0, null);
                        }
                    }
                    else
                    {
                        var v = val.Type == JTokenType.Array || val.Type == JTokenType.Object ? val : val.Value<object>();
                        if (!fe.Any(x => v.Equals(x)))
                            fe.Add(v);
                    }
                }

                first = false;
            }

            if (this.UseNestedClasses)
            {
                foreach (var field in jsonFields)
                {
                    this.Names.Add(field.Key.ToLower());
                }
            }

            foreach (var field in jsonFields)
            {
                var fieldType = field.Value;
                if (fieldType.Type == JsonTypeEnum.Object)
                {
                    var subexamples = new List<JObject>(examples.Length);
                    foreach (var obj in examples)
                    {
                        JToken value;
                        if (obj.TryGetValue(field.Key, out value))
                        {
                            if (value.Type == JTokenType.Object)
                            {
                                subexamples.Add((JObject)value);
                            }
                        }
                    }

                    fieldType.AssignName(this.CreateUniqueClassName(field.Key), field.Key);
                    this.GenerateClass(subexamples.ToArray(), fieldType);
                }

                if (fieldType.InternalType != null && fieldType.InternalType.Type == JsonTypeEnum.Object)
                {
                    var subexamples = new List<JObject>(examples.Length);
                    foreach (var obj in examples)
                    {
                        JToken value;
                        if (obj.TryGetValue(field.Key, out value))
                        {
                            if (value.Type == JTokenType.Array)
                            {
                                foreach (var item in (JArray)value)
                                {
                                    if (!(item is JObject)) throw new NotSupportedException("Arrays of non-objects are not supported yet.");
                                    subexamples.Add((JObject)item);
                                }
                            }
                            else if (value.Type == JTokenType.Object)
                            {
                                foreach (var item in (JObject)value)
                                {
                                    if (!(item.Value is JObject)) throw new NotSupportedException("Arrays of non-objects are not supported yet.");

                                    subexamples.Add((JObject)item.Value);
                                }
                            }
                        }
                    }

                    field.Value.InternalType.AssignName(this.CreateUniqueClassNameFromPlural(field.Key),
                            this.ConvertPluralToSingle(field.Key));
                    this.GenerateClass(subexamples.ToArray(), field.Value.InternalType);
                }
            }

            type.Fields = jsonFields.Select(x => new FieldInfo(this, x.Key, x.Value, this.UsePascalCase, fieldExamples[x.Key])).ToList();

            this.FeedBack?.Invoke($"Generating class {type.AssignedName}");
            this.Types.Add(type);
        }

        public IList<JsonType> Types { get; private set; }

        private HashSet<string> Names = new HashSet<string>();

        private string CreateUniqueClassName(string name)
        {
            name = ToTitleCase(name);

            var finalName = name;
            var i = 2;
            while (this.Names.Any(x => x.Equals(finalName, StringComparison.OrdinalIgnoreCase)))
            {
                finalName = name + i.ToString();
                i++;
            }

            this.Names.Add(finalName);
            return finalName;
        }

        private string CreateUniqueClassNameFromPlural(string plural)
        {
            plural = ToTitleCase(plural);
            return plural;
            //return CreateUniqueClassName(pluralizationService.Singularize(plural));
        }

        private string ConvertPluralToSingle(string plural)
        {
            plural = ToTitleCase(plural);
            return plural;
            //return pluralizationService.Singularize(plural);
        }

        internal static string ToTitleCase(string str)
        {
            var sb = new StringBuilder(str.Length);
            var flag = true;

            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(flag ? char.ToUpper(c) : c);
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }

            return sb.ToString();
        }

        public bool HasSecondaryClasses
        {
            get { return this.Types.Count > 1; }
        }
    }
}
