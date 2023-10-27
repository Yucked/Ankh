use std::fs::File;
use serde::{
    Serialize,
    Deserialize,
};

#[derive(Serialize, Deserialize)]
struct Configuration {
    database: DatabaseConfiguration,
    host: String,
    port: u16,
}

#[derive(Serialize, Deserialize)]
struct DatabaseConfiguration {
    username: String,
    password: String,
    hostname: String,
    port: u16,
    name: String,
}

pub fn load_config() {
    let cwd = std::env::current_dir().expect("Unsure about cwd");
    let config_path = cwd.join("configuration.json");

    if config_path.exists() {
        let file = File::create(config_path).unwrap();
        //serde::Serialize::serialize()
    }
}