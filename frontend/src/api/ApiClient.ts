import axios, {
  type AxiosRequestConfig,
  type AxiosResponse,
  type AxiosInstance,
} from "axios";

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

const axiosInstance: AxiosInstance = axios.create({
  baseURL: BASE_URL,
  headers: { "Content-Type": "application/json" },
  timeout: 10000,
});

axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) config.headers["Authorization"] = `Bearer ${token}`;
    return config;
  },
  (error) => Promise.reject(error)
);

axiosInstance.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error) => {
    console.error("API Error:", error.response?.data || error.message);
    return Promise.reject(error);
  }
);

export const get = async <TResponse>(
  url: string,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<TResponse>> => {
  return axiosInstance.get<TResponse>(url, config);
};

export const post = async <TRequest, TResponse>(
  url: string,
  data: TRequest,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<TResponse>> => {
  return axiosInstance.post<TResponse>(url, data, config);
};

export const put = async <TRequest, TResponse>(
  url: string,
  data: TRequest,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<TResponse>> => {
  return axiosInstance.put<TResponse>(url, data, config);
};

export const patch = async <TRequest, TResponse>(
  url: string,
  data: TRequest,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<TResponse>> => {
  return axiosInstance.patch<TResponse>(url, data, config);
};

export const remove = async <TResponse>(
  url: string,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<TResponse>> => {
  return axiosInstance.delete<TResponse>(url, config);
};
