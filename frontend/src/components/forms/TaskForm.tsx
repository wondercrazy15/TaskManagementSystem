"use client";

import { forwardRef, useImperativeHandle } from "react";
import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { taskSchema, type TaskFormData } from "@/util/validators";
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Textarea } from "../ui/textarea";
import {
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
} from "../ui/select";
import { MultiSelect } from "../common/MultiSelect";

export interface AssigneeOption {
  value: string;
  label: string;
}

interface TaskFormProps {
  assignees: AssigneeOption[];
  defaultValues?: Partial<TaskFormData>;
  onSubmit: (data: TaskFormData) => void | Promise<void>;
}

export interface TaskFormRef {
  submit: () => void;
}

const TaskForm = forwardRef<TaskFormRef, TaskFormProps>(
  ({ defaultValues = {}, onSubmit, assignees }, ref) => {
    const fullDefaultValues: TaskFormData = {
      title: defaultValues?.title ?? "",
      description: defaultValues?.description ?? "",
      taskStatusId: defaultValues?.taskStatusId ?? "0",
      priority: defaultValues?.priority ?? "Low",
      assigneeId: defaultValues?.assigneeId ?? [],
    };

    const {
      register,
      handleSubmit,
      control,
      formState: { errors },
    } = useForm<TaskFormData>({
      resolver: zodResolver(taskSchema),
      defaultValues: fullDefaultValues,
    });

    useImperativeHandle(ref, () => ({
      submit: () => handleSubmit(onSubmit)(),
    }));

    return (
      <form className="flex flex-col gap-y-4">
        <div>
          <Label>Title</Label>
          <Input {...register("title")} />
          {errors.title && <p className="error-msg">{errors.title.message}</p>}
        </div>

        <div>
          <Label>Description</Label>
          <Textarea {...register("description")} rows={4} />
          {errors.description && (
            <p className="error-msg">{errors.description.message}</p>
          )}
        </div>

        <div>
          <Label>Task Status</Label>
          <Controller
            name="taskStatusId"
            control={control}
            render={({ field }) => (
              <Select value={field.value} onValueChange={field.onChange}>
                <SelectTrigger>
                  <SelectValue placeholder="Select priority" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="1">TODO</SelectItem>
                  <SelectItem value="2">In Progress</SelectItem>
                  <SelectItem value="3">Done</SelectItem>
                </SelectContent>
              </Select>
            )}
          />
          {errors.taskStatusId && (
            <p className="error-msg">{errors.taskStatusId.message}</p>
          )}
        </div>

        <div>
          <Label>Priority</Label>
          <Controller
            name="priority"
            control={control}
            render={({ field }) => (
              <Select value={field.value} onValueChange={field.onChange}>
                <SelectTrigger>
                  <SelectValue placeholder="Select priority" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="Low">Low</SelectItem>
                  <SelectItem value="Medium">Medium</SelectItem>
                  <SelectItem value="High">High</SelectItem>
                </SelectContent>
              </Select>
            )}
          />
          {errors.priority && (
            <p className="error-msg">{errors.priority.message}</p>
          )}
        </div>

        <div>
          <Label>Assignees</Label>
          <Controller
            control={control}
            name="assigneeId"
            render={({ field }) => (
              <MultiSelect
                options={assignees}
                values={field.value || []}
                onChange={field.onChange}
              />
            )}
          />
          {errors.assigneeId && (
            <p className="error-msg">{errors.assigneeId.message}</p>
          )}
        </div>
      </form>
    );
  }
);

TaskForm.displayName = "TaskForm";
export default TaskForm;
