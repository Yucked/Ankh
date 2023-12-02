use std::fs;
use serde::{
    Serialize,
    Deserialize,
};

#[derive(Default, Serialize, Deserialize)]
pub struct Configuration {
    pub database: DatabaseConfiguration,
    pub hostname: String,
    pub port: u16,
}

#[derive(Default, Serialize, Deserialize)]
pub struct DatabaseConfiguration {
    pub username: String,
    pub password: String,
    pub hostname: String,
    pub port: u16,
    pub name: String,
}

pub fn load_config() -> Configuration {
    let cwd = std::env::current_dir().expect("Unsure about cwd");
    let config_path = cwd.join("configuration.json");

    if !config_path.exists() {
        let config: Configuration = Default::default();

        let json = serde_json::to_string_pretty(&config)
            .expect("Failed to serialize configuration to string");
        fs::write(config_path, json)
            .expect("Failed to write configuration");
        return config;
    }

    let reader = fs::read_to_string(config_path)
        .expect("Failed to read json");

    let config = serde_json::from_str(&reader)
        .expect("Failed to deserialize json to struct.");

    return config;
}