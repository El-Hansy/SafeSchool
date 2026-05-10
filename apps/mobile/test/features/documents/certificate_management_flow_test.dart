import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/documents/documents.dart';

void main() {
  test('certificate management lists guardian-visible certificates', () async {
    final repo = DocumentsRepository();
    final certificates = await repo.certificates();

    expect(certificates.single.reference, 'CERT-1');
    expect(certificates.single.title, contains('Attendance certificate'));
  });
}
