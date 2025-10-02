import React from 'react';
import clsx from 'clsx';

type ButtonVariant = 'primary' | 'dark' | 'none';

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  loading?: boolean;
  className?: string;
  children: React.ReactNode;
}

const variantClasses: Record<ButtonVariant, string> = {
  primary: 'primary-btn',
  dark: 'dark-btn',
  none: ''
};

export const Button: React.FC<ButtonProps> = ({
  variant = 'primary',
  loading = false,
  className = '',
  children,
  disabled,
  ...props
}) => {
  const baseClasses = clsx(
    variantClasses[variant],
    'uppercase w-[150px] h-[45px] relative',
    {
      'opacity-50 cursor-not-allowed': loading || disabled,
    },
    className
  );

  return (
    <button
      type="button"
      className={baseClasses}
      disabled={loading || disabled}
      {...props}
    >
      {loading ? (
        <div className="relative z-[2] flex items-center justify-center gap-3">
            {children}<span className='btn-spinner'></span></div>
      ) : ( 
        <span className="relative z-[2]">{children}</span>
      )}
    </button>
  );
};
