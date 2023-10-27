mod config;
mod types;

use surrealdb::{
    Surreal,
    engine::remote::ws::Ws,
    opt::auth::Root,
};

#[tokio::main]
async fn main() {

    connect_to_surreal()
}

async fn connect_to_surreal() {
    // Connect to the server
    let db = Surreal::new::<Ws>("").await?;

    // Signin as a namespace, database, or root user
    db.signin(Root {
        username: "",
        password: "",
    })
        .await?;

    // Select a specific namespace / database
    db.use_ns("test").use_db("test").await?;
}