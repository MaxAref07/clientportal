export interface RequestMagicLinkRequest {
  email: string;
}
export interface RequestMagicLinkResponse {
  token: string;
}
export interface VerifyMagicLinkRequest {
  token: string;
}
export interface AuthTokenResponse {
  accessToken: string;
  expiresAt: string;
}
