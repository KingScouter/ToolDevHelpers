namespace CommonLib.Models
{
    public class ProjectConfigMap
    {
        public List<BaseProjectConfig> Configs { get; set; } = [];

        /// <summary>
        /// Checks if a given key exists in the list.
        /// </summary>
        /// <param name="key">Key to check for</param>
        /// <returns>True if the key exists in the list, otherwise false</returns>
        public bool HasKey(string key)
        {
            return Configs.Any(elem => elem.key == key);
        }

        /// <summary>
        /// Retrieve a config from the list by its key.
        /// </summary>
        /// <param name="key">Key of the config to retrieve</param>
        /// <returns>Retrieved config, or false if the key could not be found.</returns>
        public BaseProjectConfig? Get(string key)
        {
            return Configs.Find(elem => elem.key == key);
        }

        /// <summary>
        /// Adds a config to the list
        /// </summary>
        /// <param name="config">Config to add</param>
        /// <returns>True if the config got added successfully, otherwise false (e.g. config already exists)</returns>
        public bool Add(BaseProjectConfig config)
        {
            if (HasKey(config.key))
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
            int idx = Configs.FindIndex(elem => elem.key == key);
            if (idx >= 0)
            {
                Configs.RemoveAt(idx);
                return true;
            }
            return false;
        }
    }
}
