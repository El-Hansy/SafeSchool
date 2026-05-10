class MobileApiClient {
  const MobileApiClient();

  List<MobileRoleWorkspace> fetchWorkspaces() => MobileRoleWorkspace.demo;

  MobileRelease currentRelease({int versionCode = 1200}) {
    return MobileRelease(
      id: 'release-12',
      versionName: '12.0.0',
      versionCode: 1200,
      updateRequired: versionCode < 1200,
      checksum: 'sha256-demo-phase12',
    );
  }
}

class MobileRoleWorkspace {
  const MobileRoleWorkspace({
    required this.roleCode,
    required this.label,
    required this.arabicLabel,
    required this.actions,
    required this.offlineAllowed,
  });

  final String roleCode;
  final String label;
  final String arabicLabel;
  final List<String> actions;
  final bool offlineAllowed;

  static const demo = <MobileRoleWorkspace>[
    MobileRoleWorkspace(roleCode: 'guardian', label: 'Guardian', arabicLabel: 'ولي الأمر', actions: ['View students', 'Submit request', 'Submit complaint'], offlineAllowed: false),
    MobileRoleWorkspace(roleCode: 'student', label: 'Student', arabicLabel: 'الطالب', actions: ['Learning', 'Messages', 'Documents'], offlineAllowed: false),
    MobileRoleWorkspace(roleCode: 'transport_driver', label: 'Transport driver', arabicLabel: 'سائق الحافلة', actions: ['Trip', 'Boarding', 'Drop'], offlineAllowed: true),
    MobileRoleWorkspace(roleCode: 'gate_access', label: 'Gate/access staff', arabicLabel: 'بوابة المدرسة', actions: ['NFC scan', 'QR scan'], offlineAllowed: true),
    MobileRoleWorkspace(roleCode: 'canteen_cashier', label: 'Canteen cashier', arabicLabel: 'المقصف', actions: ['Wallet scan', 'Charge'], offlineAllowed: true),
    MobileRoleWorkspace(roleCode: 'teacher', label: 'Teacher', arabicLabel: 'المعلم', actions: ['Class', 'Behavior'], offlineAllowed: false),
    MobileRoleWorkspace(roleCode: 'medical_staff', label: 'Medical staff', arabicLabel: 'العيادة', actions: ['Medical profile', 'Emergency'], offlineAllowed: true),
    MobileRoleWorkspace(roleCode: 'complaint_handler', label: 'Complaint handler', arabicLabel: 'الشكاوى', actions: ['Triage', 'Escalate'], offlineAllowed: false),
    MobileRoleWorkspace(roleCode: 'communication_sender', label: 'Communication sender', arabicLabel: 'الرسائل', actions: ['Broadcast', 'Direct message'], offlineAllowed: false),
    MobileRoleWorkspace(roleCode: 'document_administrator', label: 'Document administrator', arabicLabel: 'الوثائق', actions: ['Documents', 'Certificates'], offlineAllowed: false),
    MobileRoleWorkspace(roleCode: 'school_administrator', label: 'School administrator', arabicLabel: 'إدارة المدرسة', actions: ['Roles', 'Release'], offlineAllowed: false),
    MobileRoleWorkspace(roleCode: 'platform_support', label: 'Platform support', arabicLabel: 'الدعم', actions: ['Diagnostics', 'Audit'], offlineAllowed: false),
  ];
}

class MobileRelease {
  const MobileRelease({
    required this.id,
    required this.versionName,
    required this.versionCode,
    required this.updateRequired,
    required this.checksum,
  });

  final String id;
  final String versionName;
  final int versionCode;
  final bool updateRequired;
  final String checksum;
}
