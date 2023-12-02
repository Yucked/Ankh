use axum::{
    response::Json,
};
use serde_json::{Value, json};
use std::time::{SystemTime, UNIX_EPOCH};
use axum::extract::Path;
use crate::REQ_CLIENT;

pub async fn ping() -> Json<Value> {
    let start = SystemTime::now();
    let epoch = start
        .duration_since(UNIX_EPOCH)
        .expect("Time went backwards");

    Json(json!( {
        "data": {
            "time": epoch.as_millis()
        }
    }))
}

pub async fn get_avatarcard(
    Path(user_id): Path<i32>) -> Json<Value> {
    let request_url = format!("https://client-dynamic.imvu.com/api/avatarcard.php?cid={}&viewer_cid={}",
                              user_id,
                              user_id);

    let response = reqwest::get(request_url)
        .await
        .expect("Failed to get avatarcard");

    let json = response
        .json()
        .await
        .expect("Failed to turn response to json");

    Json(json)
}

pub async fn get_user_id(
    Path(username): Path<String>) -> Json<Value> {
    let request_url = "http://secure.imvu.com//catalog/skudb/gateway.php";

    let body = format!(r#"
<?xml version='1.0'?>
<methodCall>
	<methodName>gateway.getUserIdForAvatarName</methodName>
	<params>
		<param>
			<value>
				<string>{}</string>
			</value>
		</param>
	</params>
</methodCall>
"#, username.as_str());

    let response = REQ_CLIENT
        .get()
        .unwrap()
        .post(request_url)
        .header("Content-Type", "application/xml")
        .body(body)
        .send()
        .await
        .expect("Failed to get avatarcard");

    let json = response
        .text()
        .await
        .expect("Failed to turn response to json");


    Json(json!( {
        "data": {
            "user_id": json[106..json.find("</int>").unwrap()]
        }
    }))
}