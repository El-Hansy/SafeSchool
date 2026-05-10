import { mobileStyles } from "./MobileAdminLayout";

export function MobileReleaseNotesEditor() {
  return (
    <section style={mobileStyles.panel}>
      <h2>Release notes</h2>
      <p>English: SafeSchool 012 role-based mobile app release.</p>
      <p dir="rtl">العربية: إصدار تطبيق الجوال المعتمد على الأدوار للمرحلة 12.</p>
    </section>
  );
}
