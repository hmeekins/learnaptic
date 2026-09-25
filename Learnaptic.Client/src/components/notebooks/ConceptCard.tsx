import type { Concept } from "@/types/Concepts/Concept";
import ConceptEditor from "./ConceptEditor";

type ConceptCardProps = {
  concept: Concept;
};

function ConceptCard({ concept }: ConceptCardProps) {
  return (
    <section id={`concept-${concept.id}`}>
      <h2>{concept.title}</h2>
      <ConceptEditor content={concept.content} />
    </section>
  );
}

export default ConceptCard;
