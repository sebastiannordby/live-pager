export type LoginRequest = {
  username: string;
  password: string;
};

export type LoginResponse = {
  jwtToken: string | null;
};
