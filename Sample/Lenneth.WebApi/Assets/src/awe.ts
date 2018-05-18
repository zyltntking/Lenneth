// Copyright Lenneth.

export function SetToken(token: string): void {
  sessionStorage.setItem("Token", token);
}

export function GetToken(): string | null {
  return sessionStorage.getItem("Token");
}

export function SetExpire(expire: string): void {
  sessionStorage.setItem("Expire", expire);
}

export function GetExpire(): string | null {
  return sessionStorage.getItem("Expire");
}
