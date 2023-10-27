use serde::{
    Serialize,
    Deserialize,
};

#[derive(Debug, Serialize, Deserialize)]
struct Config {
    name: String,
    comfy: bool,
    foo: i64,
}

fn load_config() {
    let cfg: Config = confy::load("");
}