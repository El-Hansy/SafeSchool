import 'package:flutter/material.dart';

import '../../app/mobile_theme.dart';
import '../../core/api/mobile_api_client.dart';
import '../role_workspaces/role_workspace_registry.dart';

class SafeSchoolDemoApp extends StatefulWidget {
  const SafeSchoolDemoApp({
    super.key,
    this.apiClient = const MobileApiClient(),
    this.initialLanguageCode = 'en',
    this.initialRoleCode = 'guardian',
  });

  final MobileApiClient apiClient;
  final String initialLanguageCode;
  final String initialRoleCode;

  @override
  State<SafeSchoolDemoApp> createState() => _SafeSchoolDemoAppState();
}

class _SafeSchoolDemoAppState extends State<SafeSchoolDemoApp> {
  late String _languageCode;

  @override
  void initState() {
    super.initState();
    _languageCode = widget.initialLanguageCode;
  }

  @override
  Widget build(BuildContext context) {
    final textDirection =
        _languageCode == 'ar' ? TextDirection.rtl : TextDirection.ltr;
    return MaterialApp(
      title: 'SafeSchool NFC',
      debugShowCheckedModeBanner: false,
      theme: buildSafeSchoolTheme(textDirection),
      home: SafeSchoolDemoHome(
        apiClient: widget.apiClient,
        languageCode: _languageCode,
        initialRoleCode: widget.initialRoleCode,
        onLanguageChanged: (next) => setState(() => _languageCode = next),
      ),
    );
  }
}

class SafeSchoolDemoHome extends StatefulWidget {
  const SafeSchoolDemoHome({
    super.key,
    required this.apiClient,
    required this.languageCode,
    required this.initialRoleCode,
    required this.onLanguageChanged,
  });

  final MobileApiClient apiClient;
  final String languageCode;
  final String initialRoleCode;
  final ValueChanged<String> onLanguageChanged;

  @override
  State<SafeSchoolDemoHome> createState() => _SafeSchoolDemoHomeState();
}

class _SafeSchoolDemoHomeState extends State<SafeSchoolDemoHome> {
  List<MobileRoleWorkspace> _roles = MobileRoleWorkspaceRegistry.all;
  final _requestController = TextEditingController(
      text: 'Please approve early pickup today at 12:30.');
  final _complaintController =
      TextEditingController(text: 'The morning bus arrived late at Stop 2.');

  String _selectedRole = 'guardian';
  MobileProfile? _profile;
  MobileRelease? _release;
  InstallEventResult? _installEvent;
  bool _isBootstrapping = false;
  int _bootstrapRun = 0;
  String _connectionStatus = 'Demo data';
  String? _bootstrapError;
  bool _offlineMode = false;
  bool _tripStarted = true;
  bool _onBus = true;
  bool _insideCampus = true;
  bool _guardianCanSeeExactLocation = true;
  int _etaMinutes = 11;
  int _scansToday = 246;
  int _requests = 1;
  int _complaints = 1;
  int _documents = 4;
  int _notifications = 5;
  int _queuedActions = 0;
  int _starBalance = 18;
  double _walletBalance = 182.50;
  String _lastStop = 'Main Gate';
  String _attendanceStatus = 'Present';
  String _medicalStatus = 'No active alert';
  final List<String> _timeline = [
    '07:04 - Amina boarded NORTH-AM bus using NFC-AMINA-001',
    '07:31 - Gate NFC accepted at Main Gate',
    '10:20 - Lunch meal charged SAR 8.50 at North POS 01',
    '11:15 - Math assignment submitted',
  ];

  bool get _isArabic => widget.languageCode == 'ar';

  @override
  void initState() {
    super.initState();
    _selectedRole = widget.initialRoleCode;
    _connectionStatus =
        widget.apiClient.isConfigured ? 'API connecting' : 'Demo data';
    _bootstrapMobileContext();
  }

  @override
  void didUpdateWidget(covariant SafeSchoolDemoHome oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (oldWidget.languageCode != widget.languageCode ||
        oldWidget.apiClient.baseUrl != widget.apiClient.baseUrl ||
        oldWidget.apiClient.schoolAccountId !=
            widget.apiClient.schoolAccountId) {
      _bootstrapMobileContext();
    }
  }

  @override
  void dispose() {
    _requestController.dispose();
    _complaintController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final direction = _isArabic ? TextDirection.rtl : TextDirection.ltr;
    return Directionality(
      textDirection: direction,
      child: Scaffold(
        appBar: AppBar(
          title: Text(_text('SafeSchool NFC', 'تطبيق المدرسة الآمن')),
          actions: [
            IconButton(
              tooltip: _text('Toggle language', 'تغيير اللغة'),
              onPressed: () =>
                  widget.onLanguageChanged(_isArabic ? 'en' : 'ar'),
              icon: const Icon(Icons.language),
            ),
          ],
        ),
        body: SafeArea(
          child: ListView(
            key: const Key('demo-scroll'),
            padding: const EdgeInsets.fromLTRB(16, 8, 16, 24),
            children: [
              _hero(),
              const SizedBox(height: 12),
              _roleSelector(),
              const SizedBox(height: 12),
              _modeBar(),
              const SizedBox(height: 12),
              _bootstrapCard(),
              const SizedBox(height: 12),
              _roleContent(),
              const SizedBox(height: 12),
              _timelineCard(),
            ],
          ),
        ),
      ),
    );
  }

  Widget _hero() {
    return DecoratedBox(
      decoration: BoxDecoration(
        color: Theme.of(context).colorScheme.primaryContainer,
        borderRadius: BorderRadius.circular(12),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              _text('Guardian live view', 'عرض ولي الأمر المباشر'),
              style: Theme.of(context).textTheme.labelLarge,
            ),
            const SizedBox(height: 6),
            Text(
              _text('Amina Hassan', 'أمينة حسن'),
              style: Theme.of(context)
                  .textTheme
                  .headlineSmall
                  ?.copyWith(fontWeight: FontWeight.w800),
            ),
            const SizedBox(height: 8),
            Wrap(
              spacing: 8,
              runSpacing: 8,
              children: [
                _statusChip(Icons.badge, 'NFC-AMINA-001'),
                _statusChip(Icons.school, _attendanceStatus),
                _statusChip(
                    Icons.directions_bus, _onBus ? 'On bus' : 'Not onboard'),
                _statusChip(Icons.account_balance_wallet,
                    'SAR ${_walletBalance.toStringAsFixed(2)}'),
                _statusChip(
                  Icons.cloud_done,
                  _connectionStatus,
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _roleSelector() {
    return SizedBox(
      height: 48,
      child: ListView.separated(
        key: const Key('role-selector'),
        scrollDirection: Axis.horizontal,
        itemBuilder: (context, index) {
          final role = _roles[index];
          final selected = role.roleCode == _selectedRole;
          return ChoiceChip(
            key: ValueKey('role-${role.roleCode}'),
            selected: selected,
            avatar: Icon(_roleIcon(role.roleCode), size: 18),
            label: Text(_isArabic ? role.arabicLabel : role.label),
            onSelected: (_) => _selectRole(role.roleCode),
          );
        },
        separatorBuilder: (_, __) => const SizedBox(width: 8),
        itemCount: _roles.length,
      ),
    );
  }

  Widget _bootstrapCard() {
    final releaseText = _release == null
        ? 'APK check pending'
        : 'APK ${_release!.versionName} build ${_release!.versionCode}';
    final installText = _installEvent == null
        ? 'Install event pending'
        : 'Install ${_installEvent!.nextAction}';
    final detail = _bootstrapError == null
        ? '${_profile?.activeTenantId ?? widget.apiClient.schoolAccountId} / $_selectedRole / $releaseText / $installText'
        : 'Using local fallback - $_bootstrapError';

    return Card(
      elevation: 0,
      child: ListTile(
        leading: Icon(_bootstrapError == null
            ? Icons.verified_user
            : Icons.warning_amber_rounded),
        title: Text(
            _isBootstrapping ? 'Mobile session loading' : 'Mobile session'),
        subtitle: Text(detail),
        trailing: _isBootstrapping
            ? const SizedBox(
                width: 18,
                height: 18,
                child: CircularProgressIndicator(strokeWidth: 2),
              )
            : null,
      ),
    );
  }

  Widget _modeBar() {
    return Row(
      children: [
        Expanded(
          child: SwitchListTile(
            contentPadding: EdgeInsets.zero,
            title: Text(_text('Offline mode', 'وضع عدم الاتصال')),
            subtitle: Text(_offlineMode
                ? _text('Actions queue locally', 'يتم حفظ الإجراءات محليا')
                : _text('Actions sync now', 'تتم المزامنة الآن')),
            value: _offlineMode,
            onChanged: (value) => setState(() => _offlineMode = value),
          ),
        ),
        const SizedBox(width: 8),
        FilledButton.icon(
          onPressed: _syncQueuedActions,
          icon: const Icon(Icons.sync),
          label: Text(_text('Sync $_queuedActions', 'مزامنة $_queuedActions')),
        ),
      ],
    );
  }

  Widget _roleContent() {
    switch (_selectedRole) {
      case 'guardian':
        return _guardianWorkspace();
      case 'student':
        return _studentWorkspace();
      case 'transport_driver':
        return _driverWorkspace();
      case 'gate_access':
        return _gateWorkspace();
      case 'canteen_cashier':
        return _canteenWorkspace();
      case 'teacher':
        return _teacherWorkspace();
      case 'medical_staff':
        return _medicalWorkspace();
      case 'complaint_handler':
        return _complaintHandlerWorkspace();
      case 'communication_sender':
        return _communicationWorkspace();
      case 'document_administrator':
        return _documentWorkspace();
      case 'school_administrator':
        return _schoolAdminWorkspace();
      case 'platform_support':
        return _supportWorkspace();
      default:
        return _guardianWorkspace();
    }
  }

  Widget _guardianWorkspace() {
    return _section(
      title: _text('Parent / Guardian app', 'تطبيق ولي الأمر'),
      subtitle: _text(
          'One child view for attendance, access, bus, wallet, requests, complaints, documents, and notifications.',
          'عرض موحد للحضور والدخول والحافلة والمحفظة والطلبات والشكاوى والوثائق والتنبيهات.'),
      children: [
        _metricGrid([
          _Metric('Campus', _insideCampus ? 'Inside' : 'Outside',
              Icons.meeting_room),
          _Metric('Bus ETA', '$_etaMinutes min', Icons.route),
          _Metric('Wallet', 'SAR ${_walletBalance.toStringAsFixed(2)}',
              Icons.account_balance_wallet),
          _Metric('Alerts', '$_notifications', Icons.notifications),
        ]),
        _infoTile(
            Icons.directions_bus,
            'Live bus tracking',
            _guardianCanSeeExactLocation
                ? 'NORTH-AM near $_lastStop, ETA $_etaMinutes minutes'
                : 'Amina is onboard. Exact location hidden.'),
        _infoTile(Icons.nfc, 'Last NFC event', _timeline.first),
        _infoTile(Icons.description, 'Documents',
            '$_documents available: ID card, attendance letter, fee receipt, certificate'),
        TextField(
          key: const Key('guardian-request-field'),
          controller: _requestController,
          decoration: const InputDecoration(
            labelText: 'Request',
            border: OutlineInputBorder(),
          ),
          minLines: 1,
          maxLines: 2,
        ),
        const SizedBox(height: 8),
        TextField(
          key: const Key('guardian-complaint-field'),
          controller: _complaintController,
          decoration: const InputDecoration(
            labelText: 'Complaint',
            border: OutlineInputBorder(),
          ),
          minLines: 1,
          maxLines: 2,
        ),
        _buttonWrap([
          _action('Simulate gate NFC', Icons.nfc, _recordGuardianGateScan),
          _action('Move bus / refresh ETA', Icons.location_on, _moveBus),
          _action('Top up SAR 50', Icons.add_card, () => _topUpWallet(50)),
          _action('Submit request', Icons.fact_check, _submitGuardianRequest),
          _action('Submit complaint', Icons.report_problem,
              _submitGuardianComplaint),
          _action(
              _guardianCanSeeExactLocation
                  ? 'Hide exact bus location'
                  : 'Show exact bus location',
              Icons.visibility, () {
            setState(() =>
                _guardianCanSeeExactLocation = !_guardianCanSeeExactLocation);
          }),
        ]),
      ],
    );
  }

  Widget _studentWorkspace() {
    return _section(
      title: _text('Student app', 'تطبيق الطالب'),
      subtitle: _text(
          'Student sees personal learning, messages, documents, attendance, and wallet status only.',
          'يرى الطالب التعلم والرسائل والوثائق والحضور وحالة المحفظة الخاصة به فقط.'),
      children: [
        _metricGrid([
          _Metric('Assignments', '3 due', Icons.assignment),
          _Metric('Stars', '$_starBalance', Icons.star),
          _Metric('Wallet', 'SAR ${_walletBalance.toStringAsFixed(2)}',
              Icons.lunch_dining),
          _Metric('Messages', '$_notifications', Icons.chat),
        ]),
        _infoTile(Icons.menu_book, 'Today learning',
            'Math quiz ready, Arabic reading submitted, Science video unlocked.'),
        _buttonWrap([
          _action('Submit quiz answer', Icons.check_circle,
              () => _record('Student submitted quiz answer')),
          _action('Open certificate', Icons.workspace_premium,
              () => _record('Student opened attendance certificate')),
        ]),
      ],
    );
  }

  Widget _driverWorkspace() {
    return _section(
      title: _text('Transport driver', 'سائق الحافلة'),
      subtitle: _text(
          'Driver starts trips, scans boarding/drop-off, and sends location updates.',
          'يبدأ السائق الرحلات ويمسح الصعود والنزول ويرسل تحديثات الموقع.'),
      children: [
        _metricGrid([
          _Metric('Trip', _tripStarted ? 'Active' : 'Not started',
              Icons.directions_bus),
          _Metric('Onboard', _onBus ? 'Amina' : '0', Icons.groups),
          _Metric('ETA', '$_etaMinutes min', Icons.timer),
          _Metric('Queue', '$_queuedActions', Icons.cloud_off),
        ]),
        _buttonWrap([
          _action(_tripStarted ? 'End trip' : 'Start trip', Icons.play_arrow,
              _toggleTrip),
          _action('Scan student boarding', Icons.nfc,
              () => _driverScan(boarding: true)),
          _action('Scan student drop-off', Icons.logout,
              () => _driverScan(boarding: false)),
          _action('Send location ping', Icons.my_location, _moveBus),
        ]),
      ],
    );
  }

  Widget _gateWorkspace() {
    return _section(
      title: _text('Gate / access staff', 'موظف البوابة'),
      subtitle: _text(
          'Gate staff scans NFC/QR to create access and attendance evidence.',
          'يمسح موظف البوابة NFC/QR لإنشاء دليل الدخول والحضور.'),
      children: [
        _metricGrid([
          _Metric('Scans today', '$_scansToday', Icons.qr_code_scanner),
          _Metric('Decision', _insideCampus ? 'Inside' : 'Outside',
              Icons.verified_user),
          _Metric('Mode', _offlineMode ? 'Offline' : 'Online', Icons.wifi),
          _Metric('Queue', '$_queuedActions', Icons.pending_actions),
        ]),
        _buttonWrap([
          _action('Scan NFC entry/exit', Icons.nfc, _gateScan),
          _action('Scan QR fallback', Icons.qr_code, _gateScan),
          _action('Mark manual review', Icons.rate_review,
              () => _record('Gate scan sent to manual review')),
        ]),
      ],
    );
  }

  Widget _canteenWorkspace() {
    return _section(
      title: _text('Canteen cashier', 'موظف المقصف'),
      subtitle: _text(
          'Cashier scans student credential, checks limits, and deducts wallet balance.',
          'يمسح موظف المقصف هوية الطالب ويتحقق من الحدود ويخصم من المحفظة.'),
      children: [
        _metricGrid([
          _Metric('Balance', 'SAR ${_walletBalance.toStringAsFixed(2)}',
              Icons.account_balance_wallet),
          _Metric('Terminal', 'North POS 01', Icons.point_of_sale),
          _Metric('Lunch', 'SAR 8.50', Icons.restaurant),
          _Metric('Queue', '$_queuedActions', Icons.cloud_queue),
        ]),
        _buttonWrap([
          _action('Scan and charge lunch', Icons.nfc,
              () => _chargeWallet(8.50, 'Lunch meal')),
          _action('Charge snack SAR 4', Icons.cookie,
              () => _chargeWallet(4, 'Snack')),
          _action('Queue offline purchase', Icons.cloud_off, () {
            setState(() {
              _queuedActions++;
              _timeline.insert(0, 'Queued POS purchase for NFC-AMINA-001');
            });
          }),
        ]),
      ],
    );
  }

  Widget _teacherWorkspace() {
    return _section(
      title: _text('Teacher workspace', 'مساحة المعلم'),
      subtitle: _text(
          'Teacher records class learning and behavior without seeing guardian-only actions.',
          'يسجل المعلم التعلم والسلوك دون إجراءات ولي الأمر.'),
      children: [
        _metricGrid([
          _Metric('Class', 'Grade 4A', Icons.class_),
          _Metric('Stars', '$_starBalance', Icons.star),
          _Metric('Assignments', '3', Icons.assignment),
          _Metric('Audit', '${_timeline.length}', Icons.history),
        ]),
        _buttonWrap([
          _action('Award behavior star', Icons.star, () {
            setState(() => _starBalance++);
            _record('Teacher awarded one behavior star to Amina');
          }),
          _action('Mark homework received', Icons.task_alt,
              () => _record('Teacher marked homework received')),
        ]),
      ],
    );
  }

  Widget _medicalWorkspace() {
    return _section(
      title: _text('Medical / emergency', 'العيادة والطوارئ'),
      subtitle: _text(
          'Medical staff can view emergency profile and create incident evidence.',
          'يرى موظف العيادة ملف الطوارئ ويسجل دليل الحالة.'),
      children: [
        _infoTile(Icons.medical_information, 'Emergency profile',
            'Blood type O+, allergy: peanuts, guardian call: +966-5X-XXX-1122'),
        _infoTile(Icons.health_and_safety, 'Status', _medicalStatus),
        _buttonWrap([
          _action('Create clinic incident', Icons.local_hospital, () {
            setState(() => _medicalStatus = 'Clinic incident recorded');
            _record(
                'Medical incident recorded with guardian notification ready');
          }),
          _action('Clear medical alert', Icons.check, () {
            setState(() => _medicalStatus = 'No active alert');
            _record('Medical alert cleared');
          }),
        ]),
      ],
    );
  }

  Widget _complaintHandlerWorkspace() {
    return _section(
      title: _text('Complaint handler', 'مسؤول الشكاوى'),
      subtitle: _text(
          'Complaint handler triages, escalates, and keeps audit trail.',
          'يفرز مسؤول الشكاوى ويصعد الحالات ويحفظ السجل.'),
      children: [
        _metricGrid([
          _Metric('Open', '$_complaints', Icons.report),
          _Metric('SLA', '22h', Icons.timer),
          _Metric('Escalated', '1', Icons.trending_up),
          _Metric('Audit', '${_timeline.length}', Icons.history),
        ]),
        _buttonWrap([
          _action('Assign complaint', Icons.assignment_ind,
              () => _record('Complaint assigned to transport supervisor')),
          _action('Escalate complaint', Icons.escalator_warning,
              () => _record('Complaint escalated with SLA evidence')),
        ]),
      ],
    );
  }

  Widget _communicationWorkspace() {
    return _section(
      title: _text('Communication sender', 'مرسل الرسائل'),
      subtitle: _text(
          'Authorized staff sends targeted school messages and sees delivery evidence.',
          'يرسل الموظف المخول رسائل موجهة ويرى دليل التسليم.'),
      children: [
        _metricGrid([
          _Metric('Recipients', '128', Icons.groups),
          _Metric('Delivered', '121', Icons.mark_email_read),
          _Metric('Pending', '7', Icons.schedule_send),
          _Metric('Ack', '84%', Icons.done_all),
        ]),
        _buttonWrap([
          _action('Send bus delay notice', Icons.campaign, () {
            setState(() => _notifications++);
            _record('Bus delay notification sent to NORTH-AM guardians');
          }),
        ]),
      ],
    );
  }

  Widget _documentWorkspace() {
    return _section(
      title: _text('Document administrator', 'مسؤول الوثائق'),
      subtitle: _text(
          'Document admin publishes documents and certificates according to audience rules.',
          'ينشر مسؤول الوثائق المستندات والشهادات حسب قواعد الجمهور.'),
      children: [
        _metricGrid([
          _Metric('Documents', '$_documents', Icons.folder),
          _Metric('Certificates', '2', Icons.workspace_premium),
          _Metric('Search', 'Fresh', Icons.manage_search),
          _Metric('Audit', '${_timeline.length}', Icons.history),
        ]),
        _buttonWrap([
          _action('Publish certificate', Icons.upload_file, () {
            setState(() => _documents++);
            _record(
                'Attendance certificate published for guardian and student');
          }),
        ]),
      ],
    );
  }

  Widget _schoolAdminWorkspace() {
    return _section(
      title: _text('School administrator', 'إدارة المدرسة'),
      subtitle: _text(
          'Admin controls roles, mobile access, release, and operational dashboards.',
          'يتحكم المدير في الأدوار والوصول وإصدار التطبيق ولوحات التشغيل.'),
      children: [
        _metricGrid([
          _Metric('Roles', '12', Icons.admin_panel_settings),
          _Metric('APK', '12.0.0', Icons.android),
          _Metric('Devices', '34', Icons.devices),
          _Metric('Audits', '${_timeline.length}', Icons.receipt_long),
        ]),
        _buttonWrap([
          _action(
              'Approve mobile role',
              Icons.verified_user,
              () => _record(
                  'Admin approved mobile role grant for guardian-demo')),
          _action('Review APK release', Icons.system_update,
              () => _record('Admin reviewed APK 12.0.0 release evidence')),
        ]),
      ],
    );
  }

  Widget _supportWorkspace() {
    return _section(
      title: _text('Platform support', 'دعم المنصة'),
      subtitle: _text(
          'Support sees device sessions, version, denied access, and sync evidence.',
          'يرى الدعم جلسات الأجهزة والإصدار وحالات المنع والمزامنة.'),
      children: [
        _infoTile(Icons.android, 'Installed APK',
            'Version 12.0.0, build 1200, checksum verified'),
        _infoTile(Icons.devices, 'Device session',
            'guardian-demo / device-guardian / role guardian / tenant school-demo'),
        _infoTile(Icons.sync_problem, 'Offline queue',
            '$_queuedActions queued actions waiting for sync'),
        _buttonWrap([
          _action(
              'Create support evidence',
              Icons.bug_report,
              () => _record(
                  'Support evidence captured with correlation MOBILE-042')),
          _action('Force version check', Icons.system_update_alt,
              () => _record('Version check passed for APK 12.0.0')),
        ]),
      ],
    );
  }

  Widget _section({
    required String title,
    required String subtitle,
    required List<Widget> children,
  }) {
    return Card(
      elevation: 0,
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(title,
                style: Theme.of(context)
                    .textTheme
                    .titleLarge
                    ?.copyWith(fontWeight: FontWeight.w800)),
            const SizedBox(height: 4),
            Text(subtitle, style: Theme.of(context).textTheme.bodyMedium),
            const SizedBox(height: 16),
            ...children,
          ],
        ),
      ),
    );
  }

  Widget _metricGrid(List<_Metric> metrics) {
    return GridView.count(
      crossAxisCount: MediaQuery.sizeOf(context).width > 560 ? 4 : 2,
      childAspectRatio: 1.55,
      crossAxisSpacing: 8,
      mainAxisSpacing: 8,
      shrinkWrap: true,
      physics: const NeverScrollableScrollPhysics(),
      children: [
        for (final metric in metrics)
          DecoratedBox(
            decoration: BoxDecoration(
              border: Border.all(
                  color: Theme.of(context).colorScheme.outlineVariant),
              borderRadius: BorderRadius.circular(10),
            ),
            child: Padding(
              padding: const EdgeInsets.all(10),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(metric.icon, size: 20),
                  const SizedBox(height: 4),
                  Text(metric.value,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      style: const TextStyle(fontWeight: FontWeight.w800)),
                  Text(metric.label,
                      maxLines: 1, overflow: TextOverflow.ellipsis),
                ],
              ),
            ),
          ),
      ],
    );
  }

  Widget _infoTile(IconData icon, String title, String subtitle) {
    return ListTile(
      contentPadding: EdgeInsets.zero,
      leading: Icon(icon),
      title: Text(title),
      subtitle: Text(subtitle),
    );
  }

  Widget _buttonWrap(List<Widget> buttons) {
    return Padding(
      padding: const EdgeInsets.only(top: 8),
      child: Wrap(spacing: 8, runSpacing: 8, children: buttons),
    );
  }

  Widget _action(String label, IconData icon, VoidCallback onPressed) {
    return FilledButton.tonalIcon(
      key: ValueKey('action-$label'),
      onPressed: onPressed,
      icon: Icon(icon),
      label: Text(label),
    );
  }

  Widget _statusChip(IconData icon, String label) {
    return Chip(
      avatar: Icon(icon, size: 18),
      label: Text(label),
    );
  }

  Widget _timelineCard() {
    return Card(
      key: const Key('timeline-card'),
      elevation: 0,
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                const Icon(Icons.history),
                const SizedBox(width: 8),
                Text(_text('Live audit timeline', 'سجل التدقيق المباشر'),
                    style: Theme.of(context)
                        .textTheme
                        .titleMedium
                        ?.copyWith(fontWeight: FontWeight.w800)),
              ],
            ),
            const SizedBox(height: 8),
            for (final item in _timeline.take(8))
              Padding(
                padding: const EdgeInsets.symmetric(vertical: 4),
                child: Text(item),
              ),
          ],
        ),
      ),
    );
  }

  void _record(String event) {
    setState(() {
      if (_offlineMode) {
        _queuedActions++;
        _timeline.insert(0, 'Queued offline - $event');
      } else {
        _timeline.insert(0, event);
      }
    });
    ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text(_offlineMode ? 'Queued offline' : 'Saved')));
  }

  Future<void> _bootstrapMobileContext() async {
    final run = ++_bootstrapRun;
    setState(() {
      _isBootstrapping = true;
      _bootstrapError = null;
      _connectionStatus =
          widget.apiClient.isConfigured ? 'API connecting' : 'Demo data';
    });

    try {
      final profile = await widget.apiClient.fetchProfile(
        roleCode: _selectedRole,
        languageCode: widget.languageCode,
      );
      final workspaces = await widget.apiClient.fetchLiveWorkspaces(
        roleCode: _selectedRole,
        languageCode: widget.languageCode,
      );
      final context = await widget.apiClient.selectContext(
        roleCode: _selectedRole,
        languageCode: widget.languageCode,
        deviceId: 'device-guardian',
      );
      final release = await widget.apiClient.fetchCurrentRelease(
        roleCode: context.activeRoleCode,
        deviceId: 'device-guardian',
      );
      final installEvent = await widget.apiClient.recordInstallEvent(
        deviceId: 'device-guardian',
        releaseId: release.id,
        versionName: release.versionName,
        versionCode: release.versionCode,
        userId: profile.userId,
      );

      if (!mounted || run != _bootstrapRun) {
        return;
      }

      final nextRoles =
          workspaces.isEmpty ? MobileRoleWorkspaceRegistry.all : workspaces;
      final nextRole = nextRoles.any(
        (workspace) => workspace.roleCode == context.activeRoleCode,
      )
          ? context.activeRoleCode
          : nextRoles.first.roleCode;

      setState(() {
        _profile = profile;
        _roles = nextRoles;
        _selectedRole = nextRole;
        _release = release;
        _installEvent = installEvent;
        _connectionStatus =
            widget.apiClient.isConfigured ? 'API connected' : 'Demo data';
        _isBootstrapping = false;
        _recordTimelineOnce(
          '${widget.apiClient.isConfigured ? 'API' : 'Demo'} mobile bootstrap ready - ${profile.activeTenantId} / $nextRole / APK ${release.versionName}',
        );
      });
    } catch (error) {
      if (!mounted || run != _bootstrapRun) {
        return;
      }

      setState(() {
        _roles = MobileRoleWorkspaceRegistry.all;
        _connectionStatus = 'API unavailable';
        _bootstrapError = error.toString();
        _isBootstrapping = false;
        _recordTimelineOnce('API bootstrap failed - using local fallback');
      });
    }
  }

  void _selectRole(String roleCode) {
    setState(() => _selectedRole = roleCode);
    if (widget.apiClient.isConfigured) {
      _confirmRoleContext(roleCode);
    }
  }

  Future<void> _confirmRoleContext(String roleCode) async {
    try {
      final context = await widget.apiClient.selectContext(
        roleCode: roleCode,
        languageCode: widget.languageCode,
        deviceId: 'device-guardian',
      );

      if (!mounted || context.activeRoleCode != roleCode) {
        return;
      }

      setState(() {
        _bootstrapError = null;
        _connectionStatus = 'API connected';
        _recordTimelineOnce(
          'API role context selected - ${context.activeTenantId} / ${context.activeRoleCode}',
        );
      });
    } catch (error) {
      if (!mounted) {
        return;
      }

      setState(() {
        _connectionStatus = 'API unavailable';
        _bootstrapError = error.toString();
        _recordTimelineOnce('API role context failed - using local role');
      });
    }
  }

  void _recordTimelineOnce(String event) {
    _timeline.remove(event);
    _timeline.insert(0, event);
  }

  void _recordGuardianGateScan() {
    setState(() {
      _insideCampus = !_insideCampus;
      _attendanceStatus = _insideCampus ? 'Present' : 'Checked out';
      _scansToday++;
      _notifications++;
      _timeline.insert(0,
          'Guardian notified - Gate NFC ${_insideCampus ? 'entry' : 'exit'} for Amina');
    });
  }

  void _gateScan() {
    setState(() {
      _insideCampus = !_insideCampus;
      _attendanceStatus = _insideCampus ? 'Present' : 'Checked out';
      _scansToday++;
      if (_offlineMode) {
        _queuedActions++;
      }
      _timeline.insert(0,
          '${_offlineMode ? 'Queued' : 'Accepted'} gate scan for NFC-AMINA-001');
    });
  }

  void _moveBus() {
    const stops = [
      'Main Gate',
      'Library Road',
      'North Roundabout',
      'Stop 2',
      'Home Zone'
    ];
    final current = stops.indexOf(_lastStop);
    final next = stops[(current + 1) % stops.length];
    setState(() {
      _lastStop = next;
      _etaMinutes = (_etaMinutes - 3).clamp(2, 30).toInt();
      _timeline.insert(0,
          'Bus location updated - NORTH-AM near $_lastStop, ETA $_etaMinutes minutes');
    });
  }

  void _topUpWallet(double amount) {
    setState(() {
      _walletBalance += amount;
      _notifications++;
      _timeline.insert(
          0, 'Guardian top-up confirmed SAR ${amount.toStringAsFixed(2)}');
    });
  }

  void _chargeWallet(double amount, String item) {
    if (_walletBalance < amount) {
      _record('POS declined - insufficient balance for $item');
      return;
    }
    setState(() {
      _walletBalance -= amount;
      _timeline.insert(0,
          'POS approved - $item SAR ${amount.toStringAsFixed(2)}, balance SAR ${_walletBalance.toStringAsFixed(2)}');
    });
  }

  void _submitGuardianRequest() {
    if (_requestController.text.trim().isEmpty) {
      return;
    }
    setState(() {
      _requests++;
      _timeline.insert(0,
          'Guardian request submitted for Amina - ${_requestController.text.trim()}');
    });
    ScaffoldMessenger.of(context)
        .showSnackBar(const SnackBar(content: Text('Request submitted')));
  }

  void _submitGuardianComplaint() {
    if (_complaintController.text.trim().length < 3) {
      return;
    }
    setState(() {
      _complaints++;
      _timeline.insert(0,
          'Guardian complaint submitted for Amina - ${_complaintController.text.trim()}');
    });
    ScaffoldMessenger.of(context)
        .showSnackBar(const SnackBar(content: Text('Complaint submitted')));
  }

  void _toggleTrip() {
    setState(() {
      _tripStarted = !_tripStarted;
      _timeline.insert(
          0,
          _tripStarted
              ? 'Driver started NORTH-AM trip'
              : 'Driver ended NORTH-AM trip');
    });
  }

  void _driverScan({required bool boarding}) {
    setState(() {
      _onBus = boarding;
      if (_offlineMode) {
        _queuedActions++;
      }
      _timeline.insert(0,
          '${_offlineMode ? 'Queued' : 'Accepted'} bus ${boarding ? 'boarding' : 'drop-off'} scan for Amina');
    });
  }

  void _syncQueuedActions() {
    setState(() {
      _timeline.insert(
          0,
          _queuedActions == 0
              ? 'No queued mobile actions to sync'
              : 'Synced $_queuedActions queued mobile action(s)');
      _queuedActions = 0;
      _offlineMode = false;
    });
  }

  IconData _roleIcon(String roleCode) {
    switch (roleCode) {
      case 'guardian':
        return Icons.family_restroom;
      case 'student':
        return Icons.school;
      case 'transport_driver':
        return Icons.directions_bus;
      case 'gate_access':
        return Icons.qr_code_scanner;
      case 'canteen_cashier':
        return Icons.point_of_sale;
      case 'teacher':
        return Icons.class_;
      case 'medical_staff':
        return Icons.medical_services;
      case 'complaint_handler':
        return Icons.support_agent;
      case 'communication_sender':
        return Icons.campaign;
      case 'document_administrator':
        return Icons.folder_copy;
      case 'school_administrator':
        return Icons.admin_panel_settings;
      case 'platform_support':
        return Icons.manage_accounts;
      default:
        return Icons.apps;
    }
  }

  String _text(String en, String ar) => _isArabic ? ar : en;
}

class _Metric {
  const _Metric(this.label, this.value, this.icon);

  final String label;
  final String value;
  final IconData icon;
}
