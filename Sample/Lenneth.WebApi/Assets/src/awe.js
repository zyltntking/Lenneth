// Copyright Lenneth.
export function SetToken(token) {
    sessionStorage.setItem("Token", token);
}
export function GetToken() {
    return sessionStorage.getItem("Token");
}
export function SetExpire(expire) {
    sessionStorage.setItem("Expire", expire);
}
export function GetExpire() {
    return sessionStorage.getItem("Expire");
}
