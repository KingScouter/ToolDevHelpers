namespace CommonLib.Models
{
    public class ToolConfigMap
    {
        public List<ToolConfig> Configs { get; set; } = [];

        /// <summary>
        /// Checks if a given key exists in the list.
        /// </summary>
        /// <param name="key">Key to check for</param>
        /// <returns>True if the key exists in the list, otherwise false</returns>
        public bool HasKey(string key)
        {
            return Configs.Any(elem => elem.shortName == key);
        }

        /// <summary>
        /// Retrieve a config from the list by its key.
        /// </summary>
        /// <param name="key">Key of the config to retrieve</param>
        /// <returns>Retrieved config, or false if the key could not be found.</returns>
        public ToolConfig? Get(string key)
        {
            return Configs.Find(elem => elem.shortName == key);
        }

        /// <summary>
        /// Adds a config to the list
        /// </summary>
        /// <param name="config">Config to add</param>
        /// <returns>True if the config got added successfully, otherwise false (e.g. config already exists)</returns>
        public bool Add(ToolConfig config)
        {
            if (HasKey(config.shortName))
                return false;

            Configs.Add(config);
            return true;
        }

        /// <summary>
        /// Remove a config by its key from the list
        /// </summary>
        /// <param name="key">Key of the config to remove</param>
        /// <returns>True if the config got removed successfully, otherwise false (e.g. key not found)</returns>
        public bool Remove(string key)
        {
            int idx = Configs.FindIndex(elem => elem.shortName == key);
            if (idx >= 0)
            {
                Configs.RemoveAt(idx);
                return true;
            }
            return false;
        }
    }
}
