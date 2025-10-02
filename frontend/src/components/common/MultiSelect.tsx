"use client";

import * as React from "react";
import { Check } from "lucide-react";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandItem,
  CommandList,
} from "@/components/ui/command";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";

export interface Option {
  value: string | number;
  label: string;
  disabled?: boolean;
}

export interface OptionGroup {
  label: string;
  options: Option[];
}

interface MultiSelectProps {
  label?: string;
  placeholder?: string;
  options?: Option[];
  optionGroups?: OptionGroup[];
  values?: (string | number)[];
  onChange?: (values: (string | number)[]) => void;
  disabled?: boolean;
  className?: string;
}

export const MultiSelect: React.FC<MultiSelectProps> = ({
  label,
  placeholder = "Select options",
  options = [],
  optionGroups = [],
  values = [],
  onChange,
  disabled,
  className,
}) => {
  const [open, setOpen] = React.useState(false);

  const handleSelect = (val: string) => {
    const num = Number(val);
    const parsedVal = isNaN(num) ? val : num;

    let newValues: (string | number)[];

    if (values.includes(parsedVal)) {
      newValues = values.filter((v) => v !== parsedVal);
    } else {
      newValues = [...values, parsedVal];
    }

    onChange?.(newValues);
  };

  const selectedLabels = React.useMemo(() => {
    const flatOptions = [...options, ...optionGroups.flatMap((g) => g.options)];
    const selected = flatOptions.filter((opt) => values.includes(opt.value));

    if (selected.length === 0) return "";
    if (selected.length === 1) return selected[0].label;
    return `${selected[0].label} (${selected.length - 1} more)`;
  }, [values, options, optionGroups]);

  return (
    <div className={cn("flex flex-col gap-2 w-full", className)}>
      {label && <label className="form-label">{label}</label>}

      <Popover open={open} onOpenChange={setOpen}>
        <PopoverTrigger asChild>
          <Button
            variant="outline"
            role="combobox"
            disabled={disabled}
            className="form-input w-full justify-between text-left text-base h-auto font-normal"
          >
            <span
              className={values.length === 0 ? "text-[#C8C8C8]" : "text-black"}
            >
              {values.length > 0 ? selectedLabels : placeholder}
            </span>
            <svg
              width="14"
              height="8"
              viewBox="0 0 14 8"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
              className={cn(
                "ml-2 transition-transform duration-200",
                open ? "rotate-180" : "rotate-0"
              )}
            >
              <path
                d="M7.00006 7.99995C6.74006 7.99995 6.49006 7.89995 6.29006 7.70995L0.290059 1.70995C-0.0999414 1.31995 -0.0999414 0.689946 0.290059 0.299946C0.680059 -0.0900537 1.31006 -0.0900537 1.70006 0.299946L6.99006 5.58995L12.2801 0.299946C12.6701 -0.0900537 13.3001 -0.0900537 13.6901 0.299946C14.0801 0.689946 14.0801 1.31995 13.6901 1.70995L7.69006 7.70995C7.49006 7.90995 7.24006 7.99995 6.98006 7.99995H7.00006Z"
                fill="#9D9D9D"
              />
            </svg>
          </Button>
        </PopoverTrigger>

        <PopoverContent
          align="start"
          className="w-[var(--radix-popover-trigger-width)] p-0 rounded-md border bg-white shadow-md"
        >
          <div
            className="max-h-60 overflow-y-auto"
            onWheel={(e) => e.stopPropagation()}
          >
            <Command>
              <CommandList>
                <CommandEmpty>No options found.</CommandEmpty>

                {options.length > 0 && (
                  <CommandGroup>
                    {options.map((opt) => (
                      <CommandItem
                        key={opt.value}
                        onSelect={() => handleSelect(String(opt.value))}
                        disabled={opt.disabled}
                        className={cn(
                          "flex w-full items-center justify-between px-4 py-3 text-base cursor-pointer"
                        )}
                      >
                        <span>{opt.label}</span>
                        <Check
                          className={cn(
                            "h-4 w-4 text-primary",
                            values.includes(opt.value)
                              ? "opacity-100"
                              : "opacity-0"
                          )}
                        />
                      </CommandItem>
                    ))}
                  </CommandGroup>
                )}

                {optionGroups.map((group) => (
                  <CommandGroup
                    key={group.label}
                    heading={
                      <span className="text-base font-semibold text-primary-800 tracking-tight">
                        {group.label}
                      </span>
                    }
                  >
                    {group.options.map((opt) => (
                      <CommandItem
                        key={opt.value}
                        onSelect={() => handleSelect(String(opt.value))}
                        disabled={opt.disabled}
                        className={cn(
                          "flex w-full items-center justify-between px-4 py-2 text-base cursor-pointer"
                        )}
                      >
                        <span>{opt.label}</span>
                        <Check
                          className={cn(
                            "h-4 w-4 text-primary",
                            values.includes(opt.value)
                              ? "opacity-100"
                              : "opacity-0"
                          )}
                        />
                      </CommandItem>
                    ))}
                  </CommandGroup>
                ))}
              </CommandList>
            </Command>
          </div>
        </PopoverContent>
      </Popover>
    </div>
  );
};
