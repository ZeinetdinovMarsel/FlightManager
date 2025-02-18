import axiosInstance from "../utils/axiosInstance";
import { useAuthStore } from "../services/authService";

interface SignInResponse {
  token: string;
  refreshToken: string;
}

interface SignUpResponse {
  userId: string;
}

export const signUpUser = async (
  userName: string,
  email: string,
  password: string,
  role: number
): Promise<SignUpResponse> => {
  try {
    const response = await axiosInstance.post<SignUpResponse>(`signUp`, {
      userName,
      email,
      password,
      role,
    });
    return response.data;
  } catch (err: any) {
    throw new Error(err.response?.data?.message || "Ошибка регистрации");
  }
};

export const signInUser = async (
  email: string,
  password: string
): Promise<SignInResponse> => {
  try {
    const response = await axiosInstance.post<SignInResponse>(`signIn`, { email, password });

    const { token, refreshToken } = response.data;
    useAuthStore.getState().signIn(token, refreshToken, 900);

    return response.data;
  } catch (err: any) {
    throw new Error(err.response?.data?.message || "Ошибка входа");
  }
};
