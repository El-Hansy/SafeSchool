"use client";

import { useMemo, useState, type CSSProperties } from "react";
import {
  transportApiBaseUrl,
  transportEnumValues,
  transportHeaders,
  transportRoutes,
  type TransportOperationsData,
} from "../api/client";

type ActionState = {
  status: "idle" | "submitting" | "success" | "error";
  message: string;
};

type DataProps = {
  data: TransportOperationsData;
};

const formStyle = {
  display: "grid",
  gap: "10px",
  gridTemplateColumns: "repeat(auto-fit, minmax(190px, 1fr))",
  alignItems: "end",
} satisfies CSSProperties;

const labelStyle = {
  display: "grid",
  gap: "6px",
  color: "#344054",
  fontWeight: 700,
} satisfies CSSProperties;

const inputStyle = {
  minHeight: "38px",
  border: "1px solid #cfd7e3",
  borderRadius: "6px",
  padding: "7px 8px",
  font: "inherit",
} satisfies CSSProperties;

const buttonStyle = {
  minHeight: "40px",
  border: "1px solid #1d4ed8",
  borderRadius: "6px",
  background: "#1d4ed8",
  color: "#ffffff",
  fontWeight: 800,
  cursor: "pointer",
} satisfies CSSProperties;

export function CreateRouteAction({ data }: DataProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          transportRoutes.routes(data.schoolAccountId),
          data.schoolAccountId,
          {
            routeName: String(form.get("routeName")),
            routeCode: String(form.get("routeCode")),
            serviceDirection: Number(form.get("serviceDirection")),
            campusReference: String(form.get("campusReference")),
            plannedStartTime: String(form.get("plannedStartTime")),
            plannedEndTime: String(form.get("plannedEndTime")),
            routeStatus: transportEnumValues.routeStatus.Active,
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Route submitted to transport API.",
        );
      }}
    >
      <Field name="routeName" label="Route name" defaultValue="North Morning Route" />
      <Field name="routeCode" label="Route code" defaultValue="NORTH-AM" />
      <SelectField name="serviceDirection" label="Direction" options={[["Pickup", transportEnumValues.serviceDirection.Pickup], ["Dropoff", transportEnumValues.serviceDirection.Dropoff]]} />
      <Field name="campusReference" label="Campus" defaultValue="north-campus" />
      <Field name="plannedStartTime" label="Start time" defaultValue="06:30:00" />
      <Field name="plannedEndTime" label="End time" defaultValue="07:30:00" />
      <Field name="clientRequestId" label="Request id" defaultValue={`route-${Date.now()}`} />
      <SubmitButton state={state} label="Create route" />
      <ActionMessage state={state} />
    </form>
  );
}

export function CreateVehicleAction({ data }: DataProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          transportRoutes.vehicles(data.schoolAccountId),
          data.schoolAccountId,
          {
            vehicleName: String(form.get("vehicleName")),
            vehicleCode: String(form.get("vehicleCode")),
            plateReference: String(form.get("plateReference")),
            capacity: Number(form.get("capacity")),
            vehicleStatus: transportEnumValues.vehicleStatus.Active,
            defaultDriverReference: String(form.get("defaultDriverReference")),
            defaultSupervisorReference: String(form.get("defaultSupervisorReference")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Vehicle submitted to transport API.",
        );
      }}
    >
      <Field name="vehicleName" label="Vehicle name" defaultValue="Bus 12" />
      <Field name="vehicleCode" label="Vehicle code" defaultValue="BUS-12" />
      <Field name="plateReference" label="Plate" defaultValue="SAFE-1234" />
      <Field name="capacity" label="Capacity" defaultValue="36" />
      <Field name="defaultDriverReference" label="Driver" defaultValue="driver-yousef" />
      <Field name="defaultSupervisorReference" label="Supervisor" defaultValue="transport-supervisor" />
      <Field name="clientRequestId" label="Request id" defaultValue={`vehicle-${Date.now()}`} />
      <SubmitButton state={state} label="Create vehicle" />
      <ActionMessage state={state} />
    </form>
  );
}

export function CreateAssignmentAction({ data }: DataProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const firstRoute = data.routes[0]?.transportRouteId ?? "";
  const firstVehicle = data.vehicles[0]?.transportVehicleId ?? "";
  const firstPickup = data.routeStopSequences[0]?.routeStopSequenceId ?? "";
  const firstDrop = data.routeStopSequences[data.routeStopSequences.length - 1]?.routeStopSequenceId ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          transportRoutes.assignments(data.schoolAccountId),
          data.schoolAccountId,
          {
            studentProfileId: String(form.get("studentProfileId")),
            transportRouteId: String(form.get("transportRouteId")),
            transportVehicleId: String(form.get("transportVehicleId")) || null,
            pickupRouteStopSequenceId: String(form.get("pickupRouteStopSequenceId")) || null,
            dropRouteStopSequenceId: String(form.get("dropRouteStopSequenceId")) || null,
            serviceDirection: Number(form.get("serviceDirection")),
            validFrom: String(form.get("validFrom")),
            validTo: null,
            visibilityState: transportEnumValues.visibilityState.GuardianVisible,
            assignmentStatus: transportEnumValues.assignmentStatus.Active,
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Student bus assignment submitted to transport API.",
        );
      }}
    >
      <Field name="studentProfileId" label="Student profile" defaultValue="student-amina" />
      <RouteSelect data={data} defaultRouteId={firstRoute} />
      <VehicleSelect data={data} defaultVehicleId={firstVehicle} />
      <SequenceSelect name="pickupRouteStopSequenceId" label="Pickup stop sequence" data={data} defaultSequenceId={firstPickup} />
      <SequenceSelect name="dropRouteStopSequenceId" label="Drop stop sequence" data={data} defaultSequenceId={firstDrop} />
      <SelectField name="serviceDirection" label="Direction" options={[["Pickup", transportEnumValues.serviceDirection.Pickup], ["Dropoff", transportEnumValues.serviceDirection.Dropoff]]} />
      <Field name="validFrom" label="Valid from" defaultValue="2026-05-10" />
      <Field name="clientRequestId" label="Request id" defaultValue={`assignment-${Date.now()}`} />
      <SubmitButton state={state} label="Assign student" disabled={!firstRoute} />
      <ActionMessage state={state} />
    </form>
  );
}

export function CreateScanReadyTripAction({ data }: DataProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const firstRoute = data.routes[0]?.transportRouteId ?? "";
  const firstVehicle = data.vehicles[0]?.transportVehicleId ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          transportRoutes.scanContextTrips(data.schoolAccountId),
          data.schoolAccountId,
          {
            transportRouteId: String(form.get("transportRouteId")),
            transportVehicleId: String(form.get("transportVehicleId")),
            trackingDeviceReference: String(form.get("trackingDeviceReference")),
            serviceDirection: Number(form.get("serviceDirection")),
            tripDate: String(form.get("tripDate")),
            plannedStartTime: String(form.get("plannedStartTime")),
            driverReference: String(form.get("driverReference")),
            attendantReference: String(form.get("attendantReference")),
            supervisorReference: String(form.get("supervisorReference")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Scan-ready trip submitted to transport API.",
        );
      }}
    >
      <RouteSelect data={data} defaultRouteId={firstRoute} />
      <VehicleSelect data={data} defaultVehicleId={firstVehicle} />
      <Field name="trackingDeviceReference" label="Device" defaultValue="staff-device-12" />
      <SelectField name="serviceDirection" label="Direction" options={[["Pickup", transportEnumValues.serviceDirection.Pickup], ["Dropoff", transportEnumValues.serviceDirection.Dropoff]]} />
      <Field name="tripDate" label="Trip date" defaultValue="2026-05-10" />
      <Field name="plannedStartTime" label="Planned start" defaultValue="2026-05-10T06:30:00Z" />
      <Field name="driverReference" label="Driver" defaultValue="driver-yousef" />
      <Field name="attendantReference" label="Attendant" defaultValue="attendant-sara" />
      <Field name="supervisorReference" label="Supervisor" defaultValue="transport-supervisor" />
      <Field name="clientRequestId" label="Request id" defaultValue={`trip-${Date.now()}`} />
      <SubmitButton state={state} label="Create trip" disabled={!firstRoute || !firstVehicle} />
      <ActionMessage state={state} />
    </form>
  );
}

export function BoardingDropScanAction({ data }: DataProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const firstTrip = data.trips[0]?.transportTripId ?? "";
  const firstSequence = data.routeStopSequences[0]?.routeStopSequenceId ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          transportRoutes.scanEvents(data.schoolAccountId),
          data.schoolAccountId,
          {
            clientScanId: String(form.get("clientScanId")),
            transportTripId: String(form.get("transportTripId")),
            routeStopSequenceId: String(form.get("routeStopSequenceId")),
            credentialReference: String(form.get("credentialReference")),
            scanMethod: Number(form.get("scanMethod")),
            scanDirection: Number(form.get("scanDirection")),
            actorReference: String(form.get("actorReference")),
            trackingDeviceReference: String(form.get("trackingDeviceReference")),
            localScanTime: String(form.get("localScanTime")),
            credentialSnapshotVersion: String(form.get("credentialSnapshotVersion")),
          },
          setState,
          "Boarding/drop scan submitted to transport API.",
        );
      }}
    >
      <TripSelect data={data} defaultTripId={firstTrip} />
      <SequenceSelect name="routeStopSequenceId" label="Stop sequence" data={data} defaultSequenceId={firstSequence} />
      <Field name="credentialReference" label="Credential" defaultValue="NFC-AMINA-001" />
      <SelectField name="scanMethod" label="Method" options={[["NFC", transportEnumValues.scanMethod.Nfc], ["QR", transportEnumValues.scanMethod.Qr], ["Manual", transportEnumValues.scanMethod.Manual]]} />
      <SelectField name="scanDirection" label="Scan" options={[["Boarding", transportEnumValues.scanDirection.Boarding], ["Drop", transportEnumValues.scanDirection.Drop]]} />
      <Field name="actorReference" label="Actor" defaultValue="driver-yousef" />
      <Field name="trackingDeviceReference" label="Device" defaultValue="staff-device-12" />
      <Field name="localScanTime" label="Scan time" defaultValue="2026-05-10T06:42:00Z" />
      <Field name="credentialSnapshotVersion" label="Credential version" defaultValue="v1" />
      <Field name="clientScanId" label="Scan id" defaultValue={`scan-${Date.now()}`} />
      <SubmitButton state={state} label="Record scan" disabled={!firstTrip || !firstSequence} />
      <ActionMessage state={state} />
    </form>
  );
}

export function LocationUpdateAction({ data }: DataProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const firstTrip = data.trips[0]?.transportTripId ?? "";
  const firstSequence = data.routeStopSequences[1]?.routeStopSequenceId ?? data.routeStopSequences[0]?.routeStopSequenceId ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          transportRoutes.locationUpdates(data.schoolAccountId, String(form.get("transportTripId"))),
          data.schoolAccountId,
          {
            clientLocationId: String(form.get("clientLocationId")),
            trackingDeviceReference: String(form.get("trackingDeviceReference")),
            actorReference: String(form.get("actorReference")),
            reportedAt: String(form.get("reportedAt")),
            locationReference: String(form.get("locationReference")),
            progressState: Number(form.get("progressState")),
            nearestRouteStopSequenceId: String(form.get("nearestRouteStopSequenceId")) || null,
          },
          setState,
          "Location update submitted to transport API.",
        );
      }}
    >
      <TripSelect data={data} defaultTripId={firstTrip} />
      <Field name="trackingDeviceReference" label="Device" defaultValue="staff-device-12" />
      <Field name="actorReference" label="Actor" defaultValue="driver-yousef" />
      <Field name="reportedAt" label="Reported at" defaultValue="2026-05-10T06:52:00Z" />
      <Field name="locationReference" label="Location ref" defaultValue="geo:24.7136,46.6753" />
      <SelectField name="progressState" label="Progress" options={[["En route", transportEnumValues.locationProgressState.EnRoute], ["Approaching stop", transportEnumValues.locationProgressState.ApproachingStop], ["At stop", transportEnumValues.locationProgressState.AtStop], ["Delayed", transportEnumValues.locationProgressState.Delayed]]} />
      <SequenceSelect name="nearestRouteStopSequenceId" label="Nearest stop" data={data} defaultSequenceId={firstSequence} />
      <Field name="clientLocationId" label="Location id" defaultValue={`loc-${Date.now()}`} />
      <SubmitButton state={state} label="Update location" disabled={!firstTrip} />
      <ActionMessage state={state} />
    </form>
  );
}

export function EtaRecalculateAction({ data }: DataProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const firstTrip = data.trips[0]?.transportTripId ?? "";
  const sequenceIds = data.routeStopSequences.slice(0, 2).map((item) => item.routeStopSequenceId);

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const includeRouteStopSequenceIds = String(form.get("includeRouteStopSequenceIds")).split(",").map((item) => item.trim()).filter(Boolean);
        await submitJson(
          transportRoutes.etaRecalculate(data.schoolAccountId, String(form.get("transportTripId"))),
          data.schoolAccountId,
          {
            includeRouteStopSequenceIds,
            calculationReason: String(form.get("calculationReason")),
            sourceLocationUpdateId: String(form.get("sourceLocationUpdateId")) || null,
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "ETA recalculation submitted to transport API.",
        );
      }}
    >
      <TripSelect data={data} defaultTripId={firstTrip} />
      <Field name="includeRouteStopSequenceIds" label="Stop sequence ids" defaultValue={sequenceIds.join(",")} />
      <Field name="sourceLocationUpdateId" label="Location update id" defaultValue={data.trips[0]?.latestLocation?.transportLocationUpdateId ?? ""} required={false} />
      <Field name="calculationReason" label="Reason" defaultValue="driver location update" />
      <Field name="clientRequestId" label="Request id" defaultValue={`eta-${Date.now()}`} />
      <SubmitButton state={state} label="Recalculate ETA" disabled={!firstTrip || sequenceIds.length === 0} />
      <ActionMessage state={state} />
    </form>
  );
}

export function ManualTransportReviewAction({ data }: DataProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          transportRoutes.manualReviews(data.schoolAccountId),
          data.schoolAccountId,
          {
            sourceRecordType: String(form.get("sourceRecordType")),
            sourceRecordReference: String(form.get("sourceRecordReference")),
            reviewAction: 2,
            correctedStatus: String(form.get("correctedStatus")),
            reviewReason: String(form.get("reviewReason")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Manual review submitted to transport API.",
        );
      }}
    >
      <Field name="sourceRecordType" label="Record type" defaultValue="scan" />
      <Field name="sourceRecordReference" label="Record reference" defaultValue={data.scans[2]?.boardingDropScanEventId ?? "scan-needs-review"} />
      <Field name="correctedStatus" label="Corrected status" defaultValue="Accepted after review" />
      <Field name="reviewReason" label="Reason" defaultValue="Guardian verified student and route" />
      <Field name="clientRequestId" label="Request id" defaultValue={`review-${Date.now()}`} />
      <SubmitButton state={state} label="Submit review" />
      <ActionMessage state={state} />
    </form>
  );
}

function RouteSelect({ data, defaultRouteId }: DataProps & { defaultRouteId: string }) {
  const options = useMemo(() => data.routes.map((route) => [route.routeCode, route.transportRouteId] as const), [data.routes]);
  return <SelectField name="transportRouteId" label="Route" options={options} defaultValue={defaultRouteId} />;
}

function VehicleSelect({ data, defaultVehicleId }: DataProps & { defaultVehicleId: string }) {
  const options = useMemo(() => data.vehicles.map((vehicle) => [vehicle.vehicleCode, vehicle.transportVehicleId] as const), [data.vehicles]);
  return <SelectField name="transportVehicleId" label="Vehicle" options={options} defaultValue={defaultVehicleId} />;
}

function TripSelect({ data, defaultTripId }: DataProps & { defaultTripId: string }) {
  const options = useMemo(() => data.trips.map((trip) => [`${routeCode(data, trip.transportRouteId)} / ${vehicleCode(data, trip.transportVehicleId)}`, trip.transportTripId] as const), [data]);
  return <SelectField name="transportTripId" label="Trip" options={options} defaultValue={defaultTripId} />;
}

function SequenceSelect({ data, defaultSequenceId, label, name }: DataProps & { defaultSequenceId: string; label: string; name: string }) {
  const options = useMemo(() => data.routeStopSequences.map((sequence) => [`${sequence.sequenceNumber}. ${stopName(data, sequence.transportStopId)}`, sequence.routeStopSequenceId] as const), [data]);
  return <SelectField name={name} label={label} options={options} defaultValue={defaultSequenceId} />;
}

function SelectField({ name, label, options, defaultValue }: { name: string; label: string; options: ReadonlyArray<readonly [string, string | number]>; defaultValue?: string | number }) {
  return (
    <label style={labelStyle}>
      {label}
      <select name={name} defaultValue={defaultValue ?? options[0]?.[1] ?? ""} style={inputStyle} required>
        {options.map(([labelText, value]) => <option key={`${name}-${value}`} value={value}>{labelText}</option>)}
      </select>
    </label>
  );
}

function Field({ name, label, defaultValue, required = true }: { name: string; label: string; defaultValue: string; required?: boolean }) {
  return (
    <label style={labelStyle}>
      {label}
      <input name={name} defaultValue={defaultValue} style={inputStyle} required={required} />
    </label>
  );
}

function SubmitButton({ state, label, disabled = false }: { state: ActionState; label: string; disabled?: boolean }) {
  return <button type="submit" style={buttonStyle} disabled={state.status === "submitting" || disabled}>{label}</button>;
}

function ActionMessage({ state }: { state: ActionState }) {
  if (state.status === "idle") return null;

  return (
    <p style={{ gridColumn: "1 / -1", margin: 0, color: state.status === "error" ? "#b42318" : "#166534", fontWeight: 800 }}>
      {state.message}
    </p>
  );
}

async function submitJson(
  path: string,
  schoolAccountId: string,
  payload: object,
  setState: (state: ActionState) => void,
  successMessage: string,
) {
  const baseUrl = transportApiBaseUrl();
  if (!baseUrl) {
    setState({ status: "error", message: "Set NEXT_PUBLIC_API_BASE_URL to submit this action to the SafeSchool API." });
    return;
  }

  setState({ status: "submitting", message: "Submitting..." });
  const response = await fetch(`${baseUrl}${path}`, {
    method: "POST",
    headers: transportHeaders(schoolAccountId),
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    const body = await response.json().catch(() => null);
    setState({ status: "error", message: body?.message ?? body?.[0]?.message ?? "Transport API rejected the action." });
    return;
  }

  setState({ status: "success", message: successMessage });
}

function routeCode(data: TransportOperationsData, routeId: string) {
  return data.routes.find((route) => route.transportRouteId === routeId)?.routeCode ?? routeId;
}

function vehicleCode(data: TransportOperationsData, vehicleId: string) {
  return data.vehicles.find((vehicle) => vehicle.transportVehicleId === vehicleId)?.vehicleCode ?? vehicleId;
}

function stopName(data: TransportOperationsData, stopId: string) {
  return data.stops.find((stop) => stop.transportStopId === stopId)?.stopName ?? stopId;
}
