"use client";

import * as React from "react";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import clsx from "clsx";

export interface Option {
  value: string | number;
  label: string;
  disabled?: boolean;
}

interface CustomSelectProps {
  label?: string;
  placeholder?: string;
  options: Option[];
  value?: string | number;
  className?: string;
  onChange?: (value: string | number) => void;
  disabled?: boolean;
}

export const CustomSelect: React.FC<CustomSelectProps> = ({
  label,
  placeholder = "Select an option",
  options,
  value,
  className,
  onChange,
  disabled,
}) => {
  return (
    <div className="flex flex-col gap-2 w-full">
      {label && <label className="form-label">{label}</label>}

      <Select
        value={value !== undefined ? String(value) : undefined}
        onValueChange={(val) => {
          // detect number vs string
          const num = Number(val);
          onChange?.(isNaN(num) ? val : num);
        }}
        disabled={disabled}
      >
        <SelectTrigger
          className={clsx(
            `w-full bg-white border border-gray-300 rounded-lg px-1 py-1 text-gray-900 focus:border-primary-500`,
            className
          )}
        >
          <SelectValue placeholder={placeholder} />
        </SelectTrigger>
        <SelectContent>
          {options.map((opt) => (
            <SelectItem
              key={opt.value}
              value={String(opt.value)}
              disabled={opt.disabled}
            >
              {opt.label}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
    </div>
  );
};
