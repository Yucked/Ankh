mod configuration;
mod types;

use surrealdb::{
    Surreal,
    engine::remote::ws::Ws,
    opt::auth::Root,
};

#[tokio::main]
async fn main() -> surrealdb::Result<()> {

    // Connect to the server
    let db = Surreal::new::<Ws>("surreal.srv.lol").await?;

    // Signin as a namespace, database, or root user
    db.signin(Root {
        username: "",
        password: "",
    })
        .await?;

    // Select a specific namespace / database
    db.use_ns("test").use_db("test").await?;

    Ok(())
}

async fn connect_to_surreal() {}