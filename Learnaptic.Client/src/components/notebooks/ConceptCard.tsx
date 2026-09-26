import type { Concept } from "@/types/Concepts/Concept";
import ConceptEditor from "./ConceptEditor";

type ConceptCardProps = {
  concept: Concept;
};

function ConceptCard({ concept }: ConceptCardProps) {
  return (
    <section id={`concept-${concept.id}`}>
      <ConceptEditor content={concept.content} />
    </section>
  );
}

export default ConceptCard;
