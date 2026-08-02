type TextInputProps = {
  id: string;
  label: string;
  value: string;
  onChange: (value: string) => void;
  required?: boolean;
  maxLength?: number;
};

function TextInput(props: TextInputProps) {
  return (
    <div>
      <label htmlFor={props.id}>{props.label}</label>

      <input
        id={props.id}
        name={props.id}
        type="text"
        value={props.value}
        required={props.required}
        maxLength={props.maxLength}
        onChange={(event) => props.onChange(event.target.value)}
      />
    </div>
  );
}

export default TextInput;
