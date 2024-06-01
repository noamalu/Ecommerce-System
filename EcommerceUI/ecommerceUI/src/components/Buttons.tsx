import React from 'react';
import './style.css';

interface ButtonsProps {
  buttonsMainTextTitle: string;
  className?: string;
  icon?: string;
  size?: string;
  state?: string;
  type?: string;
}

export const Buttons: React.FC<ButtonsProps> = ({
  buttonsMainTextTitle,
  className,
  icon,
  size,
  state,
  type,
}) => {
  return (
    <button className={`buttons ${className} ${icon} ${size} ${state} ${type}`}>
      {buttonsMainTextTitle}
    </button>
  );
};
