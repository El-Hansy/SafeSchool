import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/documents/documents.dart';
void main() { test('documents_search_test', () async { final repo = DocumentsRepository(); expect(await repo.documents(), isNotEmpty); expect(await repo.certificates(), isNotEmpty); expect(await repo.search('Amina'), contains('DOC-1')); }); }
