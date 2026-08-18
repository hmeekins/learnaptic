function AuthInfoPanel() {
  return (
    <section className="hidden bg-muted/70 md:col-span-3 md:flex md:items-center md:justify-center">
      <div className="mx-auto w-full max-w-3xl px-8 lg:max-w-4xl lg:px-12 xl:max-w-5xl xl:px-16">
        <h1 className="text-4xl font-bold text-primary lg:text-5xl">
          Learnaptic
        </h1>

        <p className="mt-4 text-lg text-muted-foreground lg:text-xl">
          Turn your notes into knowledge that sticks.
        </p>

        <p className="mt-3 max-w-3xl text-muted-foreground lg:text-lg">
          Learnaptic brings your study materials together in one focused
          workspace. Build organized notebooks, turn what you&apos;re learning
          into flashcards and practice questions, and use intelligent study
          tools to understand, review, and retain difficult material.
        </p>

        <div className="mt-8 grid w-full gap-3 lg:gap-4">
          <div className="rounded-lg border bg-card p-4 lg:p-5">
            <h2 className="font-medium lg:text-lg">Organize</h2>
            <p className="mt-2 text-sm text-muted-foreground lg:text-base">
              Keep your learning material structured and easy to revisit.
            </p>
          </div>

          <div className="rounded-lg border bg-card p-4 lg:p-5">
            <h2 className="font-medium lg:text-lg">Practice</h2>
            <p className="mt-2 text-sm text-muted-foreground lg:text-base">
              Test your knowledge with interactive quizzes and flashcards.
            </p>
          </div>

          <div className="rounded-lg border bg-card p-4 lg:p-5">
            <h2 className="font-medium lg:text-lg">Understand</h2>
            <p className="mt-2 text-sm text-muted-foreground lg:text-base">
              Break complex topics into manageable pieces for better
              comprehension.
            </p>
          </div>
        </div>
      </div>
    </section>
  );
}

export default AuthInfoPanel;
