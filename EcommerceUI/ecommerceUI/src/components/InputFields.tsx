import React from 'react';
import './style.css';

interface InputFieldsProps {
  icon: boolean;
  inputFieldsMainPlaceholder: string;
  state: string;
  textBelow: boolean;
}

export const InputFields: React.FC<InputFieldsProps> = ({
  icon,
  inputFieldsMainPlaceholder,
  state,
  textBelow,
}) => {
  return (
    <div className={`input-fields ${state} ${textBelow ? 'text-below' : ''}`}>
      <input placeholder={inputFieldsMainPlaceholder} />
      {icon && <span className="icon"></span>}
    </div>
  );
};
