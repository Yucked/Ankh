mod configuration;
mod types;
mod routes;
mod imvu;


use std::sync::OnceLock;
use axum::{Router, routing::get};
use reqwest::Client;
use tokio::{
    main,
    net::TcpListener,
};
use crate::{
    routes::{ping, get_avatarcard, get_user_id},
    configuration::{Configuration},
};

static REQ_CLIENT: OnceLock<Client> = OnceLock::new();

#[main]
async fn main() {
    REQ_CLIENT.get_or_init(|| Client::new());
    let config: Configuration = configuration::load_config();
    let app = Router::new()
        .route("/", get(ping))
        .route("/v1/json/:user_id", get(get_avatarcard))
        .route("/v1/xml/:username", get(get_user_id));

    let listener = TcpListener::bind(format!("{}:{}", config.hostname, config.port))
        .await
        .unwrap();

    axum::serve(listener, app)
        .await
        .unwrap();
}