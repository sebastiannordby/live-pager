export type CreateUserRequest = {
  username: string;
  email: string;
  displayName: string | null;
  password: string;
};

export type CreateUserResponse = {
  id: string;
  username: string;
};
