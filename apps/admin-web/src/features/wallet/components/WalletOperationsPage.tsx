import type { ReactNode } from "react";
import {
  formatMoney,
  loadSchoolWalletOperations,
  type PurchaseResponse,
  type ReconciliationRunResponse,
  type SpendingLimitResponse,
  type TopUpResponse,
  type TransactionHistoryResponse,
  type WalletOperationsData,
  type WalletResponse,
} from "../api/client";
import { CashierTopUpAction, CanteenPurchaseAction } from "./WalletActionForms";

export type WalletSection = "overview" | "top-ups" | "pos" | "limits" | "settings" | "review" | "reconciliation" | "transactions";

const schoolTabs: Array<[WalletSection, string, string]> = [
  ["overview", "Overview", "/wallet"],
  ["top-ups", "Top-ups", "/wallet/top-ups"],
  ["pos", "POS", "/wallet/pos"],
  ["limits", "Limits", "/wallet/limits"],
  ["transactions", "History", "/wallet/review"],
  ["settings", "Settings", "/wallet/settings"],
  ["reconciliation", "Reconciliation", "/wallet/reconciliation"],
];

export async function WalletOperationsPage({ section = "overview" }: { section?: WalletSection }) {
  const data = await loadSchoolWalletOperations();
  const totalAvailable = data.wallets.reduce((sum, wallet) => sum + wallet.availableBalanceMinor, 0);

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 005</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>Wallet Command Center</h1>
          <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>
            Student wallets, guardian top-ups, cashier credits, canteen POS deductions, spending limits, history, reconciliation, and audit evidence.
          </p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>
        {schoolTabs.map(([key, label, href]) => (
          <a key={key} href={href} style={{ textDecoration: "none", color: section === key ? "#1d4ed8" : "#1f2937", border: `1px solid ${section === key ? "#3b82f6" : "#cbd5e1"}`, background: section === key ? "#eff6ff" : "#ffffff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 }}>{label}</a>
        ))}
      </nav>

      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))", gap: 16, marginBottom: 24 }}>
        <Metric label="Active wallets" value={String(data.wallets.filter((wallet) => wallet.walletStatus === "Active").length)} detail={`${data.wallets.length} total wallet records`} />
        <Metric label="Spendable balance" value={formatMoney(totalAvailable)} detail="Confirmed credits only" />
        <Metric label="Top-ups loaded" value={String(data.topUps.length)} detail="Guardian and cashier credits" />
        <Metric label="POS purchases" value={String(data.purchases.length)} detail="Approved, denied, and held decisions" />
        <Metric label="Open limits" value={String(data.limits.length)} detail="Guardian and school controls" />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.25fr) minmax(320px, .85fr)", gap: 18 }}>
        <Panel title={panelTitle(section)}>
          <SectionContent section={section} data={data} />
        </Panel>
        <Panel title="Controls and boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="No payment credentials" detail="Only safe provider references and normalized event ids are shown." />
            <Boundary title="No attendance mutation" detail="Wallet NFC/QR scans do not create attendance or campus access outcomes." />
            <Boundary title="No transport outcome" detail="Canteen purchase scans do not update routes, trips, ETA, or bus status." />
            <Boundary title="Audit required" detail="Sensitive wallet mutations are submitted through wallet API endpoints with tenant headers." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ section, data }: { section: WalletSection; data: WalletOperationsData }) {
  if (section === "top-ups") {
    return <Stack><CashierTopUpAction schoolAccountId={data.schoolAccountId} wallets={data.wallets} /><TopUpTable topUps={data.topUps} /></Stack>;
  }

  if (section === "pos") {
    return <Stack><CanteenPurchaseAction schoolAccountId={data.schoolAccountId} wallets={data.wallets} /><PurchaseTable purchases={data.purchases} /></Stack>;
  }

  if (section === "limits" || section === "settings") return <LimitTable limits={data.limits} />;
  if (section === "reconciliation") return <ReconciliationTable runs={data.reconciliationRuns} />;
  if (section === "review" || section === "transactions") return <TransactionTable transactions={data.transactions} />;
  return <WalletTable wallets={data.wallets} />;
}

function DataSourceBadge({ source }: { source: WalletOperationsData["dataSource"] }) {
  const api = source === "api";
  return (
    <div style={{ border: `1px solid ${api ? "#a7f3d0" : "#fed7aa"}`, background: api ? "#dcfce7" : "#fff7ed", color: api ? "#166534" : "#9a3412", padding: "12px 18px", borderRadius: 6, fontWeight: 800 }}>
      {api ? "API connected" : "API fallback"}
    </div>
  );
}

function Stack({ children }: { children: ReactNode }) {
  return <div style={{ display: "grid", gap: 18 }}>{children}</div>;
}

function Metric({ label, value, detail }: { label: string; value: string; detail: string }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 18, boxShadow: "0 1px 2px rgba(15,23,42,.08)" }}><div style={{ fontSize: 34, fontWeight: 900 }}>{value}</div><div style={{ fontWeight: 800 }}>{label}</div><div style={{ color: "#64748b", marginTop: 8 }}>{detail}</div></article>;
}

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)", overflowX: "auto" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
}

function WalletTable({ wallets }: { wallets: WalletResponse[] }) {
  return <DataTable headers={["Student", "Wallet", "Balance", "Held", "Status"]} rows={wallets.map((wallet) => [wallet.studentProfileId, wallet.walletCode, formatMoney(wallet.availableBalanceMinor, wallet.currencyCode), formatMoney(wallet.heldBalanceMinor, wallet.currencyCode), wallet.walletStatus])} />;
}

function TopUpTable({ topUps }: { topUps: TopUpResponse[] }) {
  return <DataTable headers={["Reference", "Student", "Amount", "Source", "Status"]} rows={topUps.map((topUp) => [topUp.safeReference, topUp.studentProfileId, formatMoney(topUp.amountMinor, topUp.currencyCode), topUp.source, topUp.status])} />;
}

function PurchaseTable({ purchases }: { purchases: PurchaseResponse[] }) {
  return <DataTable headers={["Purchase", "Wallet", "Amount", "Decision", "Reason"]} rows={purchases.map((purchase) => [purchase.clientPurchaseId, purchase.walletId, formatMoney(purchase.amountMinor, purchase.currencyCode), purchase.decision, purchase.decisionReason])} />;
}

function LimitTable({ limits }: { limits: SpendingLimitResponse[] }) {
  return <DataTable headers={["Wallet", "Owner", "Type", "Amount", "Status"]} rows={limits.map((limit) => [limit.walletId, limit.ownerType, limit.limitType, formatMoney(limit.amountMinor, limit.currencyCode), limit.status])} />;
}

function TransactionTable({ transactions }: { transactions: TransactionHistoryResponse[] }) {
  return <DataTable headers={["Transaction", "Wallet", "Type", "Amount", "Source"]} rows={transactions.map((tx) => [tx.transactionId, tx.walletId, tx.transactionType, formatMoney(tx.amountMinor, tx.currencyCode), tx.merchantOrSource])} />;
}

function ReconciliationTable({ runs }: { runs: ReconciliationRunResponse[] }) {
  return <DataTable headers={["Run", "Scope", "Period", "Difference", "Status"]} rows={runs.map((run) => [run.reconciliationRunId, run.scope, `${run.dateFrom} to ${run.dateUntil}`, formatMoney(run.differenceMinor), run.status])} />;
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No records returned for this tenant.</p>;

  return (
    <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 680 }}>
      <thead>
        <tr>{headers.map((header) => <th key={header} style={{ textAlign: "left", color: "#475467", fontSize: 12, textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" }}>{header}</th>)}</tr>
      </thead>
      <tbody>
        {rows.map((row) => (
          <tr key={row.join(":")}>{row.map((cell, index) => <td key={`${cell}-${index}`} style={{ padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: index === 0 ? "#101828" : "#344054", fontWeight: index === 0 ? 800 : 500 }}>{cell}</td>)}</tr>
        ))}
      </tbody>
    </table>
  );
}

function panelTitle(section: WalletSection) {
  const titles: Record<WalletSection, string> = { overview: "Wallet ledger", "top-ups": "Top-up and payment confirmation", pos: "Canteen POS", limits: "Spending limits", settings: "Wallet rule settings", review: "History and corrections", reconciliation: "Reconciliation", transactions: "Transaction history" };
  return titles[section];
}
