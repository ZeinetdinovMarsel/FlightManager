import axios from "axios";
import { useAuthStore } from "../store/auth"; 

const API_URL = "http://localhost:5183/";

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
    const response = await axios.post<SignUpResponse>(`${API_URL}signUp`, {
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
    const response = await axios.post<SignInResponse>(`${API_URL}signIn`, { email, password });
    
    const { token, refreshToken } = response.data;
    const signIn = useAuthStore.getState().signIn;
    signIn(token, refreshToken);

    return response.data;
  } catch (err: any) {
    throw new Error(err.response?.data?.message || "Ошибка входа");
  }
};
