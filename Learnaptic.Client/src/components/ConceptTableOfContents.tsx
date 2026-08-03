import type { Concept } from "../types/Concept";

type ConceptTableOfContentsProps = {
  concepts: Concept[];
};
function ConceptTableOfContents(props: ConceptTableOfContentsProps) {
  return (
    <div>
      {props.concepts.map((concept) => (
        <p key={concept.id}>{concept.title}</p>
      ))}
    </div>
  );
}

export default ConceptTableOfContents;
