export interface ApiError {
  code: string;
  message: string;
}

export interface ApiErrorResponse {
  message: string;
  errors: ApiError[];
}
