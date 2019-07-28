namespace AspNetCore.Extensions.JsonClassGenerate
{
    using System.IO;

    public interface ICodeWriter
    {
        string FileExtension { get; }

        string DisplayName { get; }

        string GetTypeName(JsonType type, IJsonClassGeneratorConfig config);

        void WriteClass(IJsonClassGeneratorConfig config, TextWriter sw, JsonType type, bool notLast);

        void WriteFileStart(IJsonClassGeneratorConfig config, TextWriter sw);

        void WriteFileEnd(IJsonClassGeneratorConfig config, TextWriter sw);

        void WriteNamespaceStart(IJsonClassGeneratorConfig config, TextWriter sw, bool root);

        void WriteNamespaceEnd(IJsonClassGeneratorConfig config, TextWriter sw, bool root);
    }
}
