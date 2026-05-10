import { readFileSync } from "node:fs";
import { join } from "node:path";
import { describe, expect, it } from "vitest";
import { learningRoutes, loadLearningOperations } from "../../src/features/learning/api/learningApi";

describe("learning operational pages", () => {
  it("uses operational learning pages instead of static demo pages", () => {
    const pages = [
      "src/app/(school)/learning/page.tsx",
      "src/app/(school)/learning/content/page.tsx",
      "src/app/(school)/learning/assignments/page.tsx",
      "src/app/(school)/learning/quizzes/page.tsx",
      "src/app/(school)/learning/stars/page.tsx",
      "src/app/(school)/learning/rewards/page.tsx",
      "src/app/(school)/learning/behavior/page.tsx",
      "src/app/(school)/learning/history/page.tsx",
      "src/app/(school)/learning/review/page.tsx",
      "src/app/(school)/learning/configuration/page.tsx",
      "src/app/(guardian)/guardian/learning/page.tsx",
      "src/app/(guardian)/guardian/learning/assignments/page.tsx",
      "src/app/(student)/student/learning/page.tsx",
      "src/app/(student)/student/learning/assignments/page.tsx",
      "src/app/(student)/student/learning/quizzes/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("LearningDemo");
      expect(source).toContain("LearningOperationsPage");
    }
  });

  it("loads fallback data for school, guardian, and student learning scopes", async () => {
    const data = await loadLearningOperations("school-demo", "student-amina");

    expect(data.dataSource).toBe("fallback");
    expect(data.metrics.length).toBeGreaterThan(0);
    expect(data.schoolActivities.length).toBeGreaterThan(0);
    expect(data.guardianActivities.length).toBeGreaterThan(0);
    expect(data.studentActivities.length).toBeGreaterThan(0);
    expect(data.boundaryNotes.join(" ")).toContain("wallet");
  });

  it("keeps learning write routes scoped and explicit", () => {
    expect(learningRoutes.submissions("school-demo")).toBe("/api/v1/schools/school-demo/learning/submissions");
    expect(learningRoutes.quizAttempts("school-demo")).toBe("/api/v1/schools/school-demo/learning/quiz-attempts");
    expect(learningRoutes.starSourceEvents("school-demo")).toBe("/api/v1/schools/school-demo/learning/stars/source-events");
    expect(learningRoutes.rewardRedemptions("school-demo")).toBe("/api/v1/schools/school-demo/learning/reward-redemptions");
    expect(learningRoutes.manualReviews("school-demo")).toBe("/api/v1/schools/school-demo/learning/manual-reviews");
  });

  it("exposes learning action forms through route helpers", () => {
    const source = readFileSync(join(process.cwd(), "src/features/learning/components/LearningActionForms.tsx"), "utf8");

    expect(source).toContain("learningRoutes.content");
    expect(source).toContain("learningRoutes.assignments");
    expect(source).toContain("learningRoutes.submissions");
    expect(source).toContain("learningRoutes.quizAttempts");
    expect(source).toContain("learningRoutes.starSourceEvents");
    expect(source).toContain("learningRoutes.rewardRedemptions");
    expect(source).toContain("learningRoutes.behavior");
    expect(source).toContain("learningRoutes.configuration");
    expect(source).toContain("learningRoutes.manualReviews");
  });

  it("keeps backend POST contracts for the demo actions", () => {
    const controllerSources = [
      "../api/src/SafeSchool.Api/Features/Learning/Content/CourseContentControllers.cs",
      "../api/src/SafeSchool.Api/Features/Learning/Assignments/AssignmentControllers.cs",
      "../api/src/SafeSchool.Api/Features/Learning/Quizzes/QuizControllers.cs",
      "../api/src/SafeSchool.Api/Features/Learning/Stars/StarRewardControllers.cs",
      "../api/src/SafeSchool.Api/Features/Learning/Behavior/BehaviorControllers.cs",
      "../api/src/SafeSchool.Api/Features/Learning/Reviews/LearningReviewControllers.cs",
    ].map((path) => readFileSync(join(process.cwd(), path), "utf8")).join("\n");

    expect(controllerSources).toContain('MapPost("/content"');
    expect(controllerSources).toContain('MapPost("/assignments"');
    expect(controllerSources).toContain('MapPost("/submissions"');
    expect(controllerSources).toContain('MapPost("/quiz-attempts"');
    expect(controllerSources).toContain('MapPost("/stars/source-events"');
    expect(controllerSources).toContain('MapPost("/reward-redemptions"');
    expect(controllerSources).toContain('MapPost("/behavior-events"');
    expect(controllerSources).toContain('MapPost("/rule-settings"');
    expect(controllerSources).toContain('MapPost("/manual-reviews"');
  });
});
