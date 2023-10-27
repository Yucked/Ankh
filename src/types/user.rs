#[derive(Debug, Serialize)]
struct User<'a> {
    first: &'a str,
    last: &'a str,
}