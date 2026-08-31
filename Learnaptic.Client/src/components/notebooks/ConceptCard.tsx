import type { Concept } from "@/types/Concepts/Concept";

type ConceptCardProps = {
  concept: Concept;
};

function ConceptCard({ concept }: ConceptCardProps) {
  return (
    <section id={`concept-${concept.id}`}>
      <h2>{concept.title}</h2>
      <p>{concept.content}</p>
    </section>
  );
}

export default ConceptCard;
