import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { registerSchema, type RegisterFormData } from "../../util/validators";
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Button } from "../ui/button";

interface RegisterFormProps {
  onSubmit: (data: RegisterFormData) => void;
}

export const RegisterForm: React.FC<RegisterFormProps> = ({ onSubmit }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  });

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className="flex flex-col gap-4 w-full max-w-md"
    >
      <div className="flex flex-col">
        <Label className="mb-2">Username</Label>
        <Input {...register("username")} placeholder="Username" />
        {errors.username && (
          <span className="text-red-500">{errors.username.message}</span>
        )}
      </div>

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

      <div className="flex flex-col">
        <Label className="mb-2">Confirm Password</Label>
        <Input
          {...register("confirmPassword")}
          placeholder="Confirm Password"
          type="password"
        />
        {errors.confirmPassword && (
          <span className="text-red-500">{errors.confirmPassword.message}</span>
        )}
      </div>

      <Button type="submit" className="mt-2 primary-btn">
        Register
      </Button>
    </form>
  );
};
