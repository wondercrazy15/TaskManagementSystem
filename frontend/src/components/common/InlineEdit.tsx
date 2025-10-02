import { useState } from "react";

interface InlineEditProps {
  value: string;
  onSave: (newValue: string) => void;
}

export const InlineEdit: React.FC<InlineEditProps> = ({ value, onSave }) => {
  const [isEditing, setIsEditing] = useState(false);
  const [text, setText] = useState(value);

  const handleBlur = () => {
    setIsEditing(false);
    if (text !== value) onSave(text);
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") handleBlur();
    if (e.key === "Escape") {
      setText(value);
      setIsEditing(false);
    }
  };

  return (
    <div className="w-full">
      {isEditing ? (
        <input
          value={text}
          onChange={(e) => setText(e.target.value)}
          onBlur={handleBlur}
          onKeyDown={handleKeyDown}
          autoFocus
          className="w-full border rounded p-1"
        />
      ) : (
        <span onClick={() => setIsEditing(true)}>{value}</span>
      )}
    </div>
  );
};
