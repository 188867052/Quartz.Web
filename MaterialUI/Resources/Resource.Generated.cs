using System.Threading;
using System.Resources;

namespace MaterialUI.Resources
{
	/// <summary>
	/// A static class used to access a specific set of resources.
	/// </summary>
	public static class EditScheduleConfigurationResource
	{
        private static ResourceManager resourceManager;
        
        /// <summary>
        /// Gets the cached ResourceManager instance used by this class.
        /// </summary>
        private static ResourceManager ResourceManager 
		{
            get 
			{
                if (object.ReferenceEquals(resourceManager, null)) 
				{
                    resourceManager = new ResourceManager("MaterialUI.Resources.EditScheduleConfigurationResource", typeof(EditScheduleConfigurationResource).Assembly);
                }
                return resourceManager;
            }
        }
        
        /// <summary>
        /// Returns the formatted resource string.
        /// </summary>
		/// <param name="key">The resource key.</param>
		/// <returns>The localized resource string.</returns>
        private static string GetResourceString(string key)
		{
			var culture = Thread.CurrentThread.CurrentCulture;
            return ResourceManager.GetString(key, culture);
        }
		
		/// <summary>
		/// Gets the localized string for AddTitle.
		/// </summary>
		public static string AddTitle { get { return GetResourceString("AddTitle"); } }

		/// <summary>
		/// Gets the localized string for CronExpression.
		/// </summary>
		public static string CronExpression { get { return GetResourceString("CronExpression"); } }

		/// <summary>
		/// Gets the localized string for EditTitle.
		/// </summary>
		public static string EditTitle { get { return GetResourceString("EditTitle"); } }

		/// <summary>
		/// Gets the localized string for EndTime.
		/// </summary>
		public static string EndTime { get { return GetResourceString("EndTime"); } }

		/// <summary>
		/// Gets the localized string for Group.
		/// </summary>
		public static string Group { get { return GetResourceString("Group"); } }

		/// <summary>
		/// Gets the localized string for HttpMethod.
		/// </summary>
		public static string HttpMethod { get { return GetResourceString("HttpMethod"); } }

		/// <summary>
		/// Gets the localized string for IntervalTime.
		/// </summary>
		public static string IntervalTime { get { return GetResourceString("IntervalTime"); } }

		/// <summary>
		/// Gets the localized string for IntervalType.
		/// </summary>
		public static string IntervalType { get { return GetResourceString("IntervalType"); } }

		/// <summary>
		/// Gets the localized string for IsEnableLable.
		/// </summary>
		public static string IsEnableLable { get { return GetResourceString("IsEnableLable"); } }

		/// <summary>
		/// Gets the localized string for LastExcuteTime.
		/// </summary>
		public static string LastExcuteTime { get { return GetResourceString("LastExcuteTime"); } }

		/// <summary>
		/// Gets the localized string for Name.
		/// </summary>
		public static string Name { get { return GetResourceString("Name"); } }

		/// <summary>
		/// Gets the localized string for NextExcuteTime.
		/// </summary>
		public static string NextExcuteTime { get { return GetResourceString("NextExcuteTime"); } }

		/// <summary>
		/// Gets the localized string for OkLable.
		/// </summary>
		public static string OkLable { get { return GetResourceString("OkLable"); } }

		/// <summary>
		/// Gets the localized string for StartTime.
		/// </summary>
		public static string StartTime { get { return GetResourceString("StartTime"); } }

		/// <summary>
		/// Gets the localized string for Status.
		/// </summary>
		public static string Status { get { return GetResourceString("Status"); } }

		/// <summary>
		/// Gets the localized string for TriggerType.
		/// </summary>
		public static string TriggerType { get { return GetResourceString("TriggerType"); } }

		/// <summary>
		/// Gets the localized string for Url.
		/// </summary>
		public static string Url { get { return GetResourceString("Url"); } }
	}
}
