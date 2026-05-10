import { MobileAdminLayout, MobilePermissionGrantForm, MobileRoleMatrix } from "../../../../features/mobile";

export default function MobileRolesPage() {
  return (
    <MobileAdminLayout title="Mobile Roles">
      <MobileRoleMatrix />
      <MobilePermissionGrantForm />
    </MobileAdminLayout>
  );
}
