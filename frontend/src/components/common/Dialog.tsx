"use client";

import * as React from "react";
import {
  Dialog,
  DialogTrigger,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogDescription,
  DialogClose,
  DialogFooter,
} from "@/components/ui/dialog";
import { Cross2Icon } from "@radix-ui/react-icons";

interface AppDialogProps {
  open?: boolean;
  onOpenChange?: (open: boolean) => void;
  trigger?: React.ReactNode;
  title?: string;
  description?: string;
  children?: React.ReactNode;
  footer?: React.ReactNode;
  showCloseButton?: boolean;
  dialogClassName?: string;
  bodyClass?: string;
  headerClassName?: string;
  footerClassName?: string;
  icon?: React.ReactNode;
}

const AppDialog: React.FC<AppDialogProps> = ({
  open,
  onOpenChange,
  trigger,
  title,
  description,
  children,
  showCloseButton = false,
  dialogClassName = "min-w-[550px]",
  footer,
  bodyClass = "",
  headerClassName = "",
  footerClassName = "",
  icon,
}) => {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      {trigger && <DialogTrigger asChild>{trigger}</DialogTrigger>}

      <DialogContent
        className={dialogClassName}
        onInteractOutside={(event) => {
          event.preventDefault();
        }}
        aria-describedby={description ? undefined : undefined}
      >
        <DialogHeader headerClassName={headerClassName}>
          {icon && icon}
          {title && <DialogTitle>{title}</DialogTitle>}
          {description && <DialogDescription>{description}</DialogDescription>}
        </DialogHeader>

        {children && (
          <div
            className={`px-[30px] max-h-[calc(100vh-320px)] overflow-auto custom-scrollbar ${bodyClass}`}
          >
            {children}
          </div>
        )}

        {footer && (
          <DialogFooter footerClassName={footerClassName}>
            {footer}
          </DialogFooter>
        )}

        {showCloseButton && (
          <DialogClose asChild>
            <button
              className="absolute right-3 top-3 inline-flex h-6 w-6 items-center justify-center rounded-full text-gray-600 hover:bg-gray-100 focus:outline-none"
              aria-label="Close"
            >
              <Cross2Icon />
            </button>
          </DialogClose>
        )}
      </DialogContent>
    </Dialog>
  );
};

export default AppDialog;
