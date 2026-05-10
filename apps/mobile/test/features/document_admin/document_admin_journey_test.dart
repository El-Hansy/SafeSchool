import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/document_admin/document_admin_workspace_screen.dart';

void main() {
  test('document admin actions require audit', () {
    expect(const DocumentAdminAction('doc-1', 'review').requiresAudit, isTrue);
  });
}
