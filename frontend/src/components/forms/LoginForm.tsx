"use client";

import * as React from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { loginSchema, type LoginFormData } from "../../util/validators";
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Button } from "../common/Button";

interface LoginFormProps {
  isLoading: boolean;
  onSubmit: (data: LoginFormData) => void;
}

export const LoginForm: React.FC<LoginFormProps> = ({
  onSubmit,
  isLoading,
}) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="flex gap-4 flex-col">
      <div className="flex flex-col">
        <Label className="mb-2">Email</Label>
        <Input {...register("email")} placeholder="Email" type="email" />
        {errors.email && (
          <span className="text-red-500">{errors.email.message}</span>
        )}
      </div>

      <div className="flex flex-col">
        <Label className="mb-2">Password</Label>
        <Input
          {...register("password")}
          placeholder="Password"
          type="password"
        />
        {errors.password && (
          <span className="text-red-500">{errors.password.message}</span>
        )}
      </div>

      <Button
        loading={isLoading}
        type="submit"
        className="w-full mt-2 btn-dark"
      >
        {isLoading}
        Login
      </Button>

      <div className="flex justify-center">
        <a href="/register">Sign Up</a>
      </div>
    </form>
  );
};
