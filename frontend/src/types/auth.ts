export interface RegisterRequest {
  userName: string;
  email: string;
  password: string;
  role: string;
}

export interface RegisterResponse {
  userName: string;
  email: string;
  password: string;
  role: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiration: string;
  emailConfirmed: boolean;
  phoneNumberConfirmed: boolean;
  id: string;
  email: string;
  phoneNumber: string | null;
  firstName: string | null;
  lastName: string | null;
  dateOfBirth: string;
  profileImage: string;
  role: string;
  createdAt: string;
}
