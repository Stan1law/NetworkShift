// Assets/Editor/GenerateOfficeFromImage_AddRooms.cs
// Editor tool: generate the office layout matching the provided reference image and add ServerRoom, Reception, Storage.
// - Creates a new root: Mission_01_FromImage
// - Outer shell (floor, 4 walls, ceiling), internal floors and walls matching the image layout
// - Adds Reception, Storage, ServerRoom in logical positions that integrate with the image plan
// - Doors are openings only (walls split left/right)
// - Validation run after generation (overlap & out-of-bounds warnings)
// - Safe to run multiple times; "Clear" menu removes the whole root
//
// Usage:
//  - Place under Assets/Editor/
//  - Run Tools > Network Shift > Generate Office From Image
//  - Clear via Tools > Network Shift > Clear Office From Image

using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public static class GenerateOfficeFromImage_AddRooms
{
    const string rootName = "Mission_01_FromImage";

    // Building footprint (chosen to give space similar to the reference)
    const float buildingWidth = 48f; // X total (-24..+24)
    const float buildingDepth = 32f; // Z total (-16..+16)
    const float wallHeight = 3.0f;
    const float wallThickness = 0.2f;

    // Floor thickness & offset to avoid z-fighting
    const float floorThickness = 0.1f;
    const float floorYOffset = 0.055f;

    [MenuItem("Tools/Network Shift/Generate Office From Image")]
    public static void Generate()
    {
        // If root exists ask to replace (user wanted to "ignore all progress and start again")
        var existing = GameObject.Find(rootName);
        if (existing != null)
        {
            if (!EditorUtility.DisplayDialog("Generate Office From Image",
                    $"A GameObject named '{rootName}' already exists. Replace it with a fresh copy?", "Replace", "Cancel"))
                return;
            Undo.DestroyObjectImmediate(existing);
        }

        Undo.IncrementCurrentGroup();
        int g = Undo.GetCurrentGroup();

        GameObject root = new GameObject(rootName);
        Undo.RegisterCreatedObjectUndo(root, "Create " + rootName);
        root.transform.position = Vector3.zero;

        // Create base floor and ceiling
        CreateFloorOrCeiling(root.transform, "Building_Floor", buildingWidth, floorThickness, buildingDepth, new Vector3(0f, floorYOffset, 0f));
        CreateFloorOrCeiling(root.transform, "Building_Ceiling", buildingWidth, floorThickness, buildingDepth, new Vector3(0f, wallHeight - floorYOffset, 0f));

        float halfW = buildingWidth * 0.5f; // 24
        float halfD = buildingDepth * 0.5f; // 16
        float wallY = wallHeight * 0.5f;

        // Outer walls
        CreateWall(root.transform, "Wall_Outer_Front", new Vector3(0f, wallY, -halfD), new Vector3(buildingWidth, wallHeight, wallThickness));
        CreateWall(root.transform, "Wall_Outer_Back", new Vector3(0f, wallY, halfD), new Vector3(buildingWidth, wallHeight, wallThickness));
        CreateWall(root.transform, "Wall_Outer_Left", new Vector3(-halfW, wallY, 0f), new Vector3(wallThickness, wallHeight, buildingDepth));
        CreateWall(root.transform, "Wall_Outer_Right", new Vector3(halfW, wallY, 0f), new Vector3(wallThickness, wallHeight, buildingDepth));

        // Layout zoning (divide depth into rows matching the image)
        // Back/top row depth = 10 (Z roughly 6..16), middle (Open Office) depth = 12 (Z roughly -6..6), front/bottom row depth = 10 (Z -16..-6)
        float backDepth = 10f;      // back row
        float middleDepth = 12f;    // open office
        float frontDepth = 10f;     // front row
        float zBackCenter = halfD - backDepth * 0.5f;    // +11
        float zMiddleCenter = (halfD - backDepth) - (middleDepth * 0.5f); // roughly -2
        float zFrontCenter = -halfD + frontDepth * 0.5f; // -11

        // Back row widths: Office1 (left), BreakRoom (center), Pantry/Toilet (right)
        // We'll split buildingWidth into 20, 16, 12 respectively to reflect the reference image
        float office1W = 20f; float breakW = 16f; float pantryW = 12f;
        float leftEdge = -halfW; // -24
        float office1CenterX = leftEdge + office1W * 0.5f; // -14
        float office1Right = leftEdge + office1W; // -4
        float breakCenterX = office1Right + breakW * 0.5f; // 4
        float pantryCenterX = breakCenterX + breakW * 0.5f + pantryW * 0.5f; // +18

        // Middle row: Open Office spans most of width, centered
        float openWidth = 36f; // leave margins
        float openCenterX = 0f;

        // Front row split: Reception (left), Storage (center), Meeting (right) — three equalish sections
        float frontSegW = buildingWidth / 3f; // 16 each
        float receptionCenterX = -halfW + frontSegW * 0.5f; // -16
        float storageCenterX = -halfW + frontSegW * 1.5f;   // 0
        float meetingCenterX = -halfW + frontSegW * 2.5f;   // +16

        // Add floors (thin slabs) for each room (no furniture)
        // Back row floors
        CreateFloor(root.transform, "Floor_Office1", office1W, backDepth, new Vector3(office1CenterX, floorYOffset, zBackCenter));
        CreateFloor(root.transform, "Floor_BreakRoom", breakW, backDepth, new Vector3(breakCenterX, floorYOffset, zBackCenter));
        // Pantry and toilet split: we'll create one floor for pantry area and one for toilet area within the rightmost block
        float pantryDepth = backDepth * 0.65f;
        float toiletDepth = backDepth - pantryDepth;
        CreateFloor(root.transform, "Floor_Pantry", pantryW, pantryDepth, new Vector3(pantryCenterX, floorYOffset, zBackCenter + (backDepth - pantryDepth) * 0.5f));
        CreateFloor(root.transform, "Floor_Toilet", pantryW, toiletDepth, new Vector3(pantryCenterX, floorYOffset, zBackCenter - (pantryDepth) * 0.5f));

        // Middle / Open Office
        CreateFloor(root.transform, "Floor_OpenOffice", openWidth, middleDepth, new Vector3(openCenterX, floorYOffset, zMiddleCenter));

        // Front row floors
        CreateFloor(root.transform, "Floor_Reception", frontSegW, frontDepth, new Vector3(receptionCenterX, floorYOffset, zFrontCenter));
        CreateFloor(root.transform, "Floor_Storage", frontSegW, frontDepth, new Vector3(storageCenterX, floorYOffset, zFrontCenter));
        CreateFloor(root.transform, "Floor_Meeting", frontSegW, frontDepth, new Vector3(meetingCenterX, floorYOffset, zFrontCenter));

        // Now add the three requested rooms:
        // ServerRoom: place a small server room adjacent to the pantry area (top-right inner corner),
        // Reception and Storage are already present above (we added Reception and Storage floors). We'll rename Storage to StorageRoom and ensure sizes.
        // Add ServerRoom floor
        float serverW = 8f;
        float serverD = 6f;
        Vector3 serverPos = new Vector3(pantryCenterX - (pantryW * 0.5f) + serverW * 0.5f, floorYOffset, zBackCenter - 2.0f);
        CreateFloor(root.transform, "Floor_ServerRoom", serverW, serverD, serverPos);

        // (We already created Reception and Storage floors above; rename Storage to StorageRoom for clarity)
        var storage = root.transform.Find("Floor_Storage");
        if (storage != null) storage.name = "Floor_StorageRoom";

        // Create internal walls and openings to match the reference:
        //  - A horizontal wall between back row and open office with an opening where the corridor is (centered)
        //  - A vertical wall dividing open office and meeting room (leaving an opening near the top for access)
        //  - Walls around Office1, Pantry/Toilet blocks, and server room (leave openings facing the corridor/center)
        float wallZBetweenBackAndOpen = zMiddleCenter + (middleDepth * 0.5f) - 0.5f; // near top edge of open office
        CreateWallWithOpening(root.transform, "Wall_Back_to_Open", new Vector3(0f, wallY, (zBackCenter + zMiddleCenter) / 2f), totalLengthX: buildingWidth - 4f, openingCenterX: 0f, openingWidth: 3.0f);

        // Vertical wall between OpenOffice and Meeting (split to leave doorway)
        float divideX = (meetingCenterX + openCenterX) * 0.5f; // near right side of open office
        CreateWall(root.transform, "Wall_Open_Meeting_Left", new Vector3(divideX - 2.0f, wallY, zMiddleCenter), new Vector3(wallThickness, wallHeight, middleDepth - 1.5f));
        CreateWall(root.transform, "Wall_Open_Meeting_Right", new Vector3(divideX + 6.0f, wallY, zMiddleCenter), new Vector3(wallThickness, wallHeight, middleDepth - 1.5f));
        // The gap between these two vertical segments forms the corridor/doorway into the meeting room.

        // Walls around Office1 (back-left)
        CreateRectangularPartition(root.transform, "Partition_Office1", office1CenterX, zBackCenter, office1W, backDepth, openingSide: RectOpeningSide.East, openingWidth: 2.5f);

        // Walls around Pantry/Toilet block (back-right)
        // We'll create a rectangular partition and leave opening toward the corridor (west side)
        CreateRectangularPartition(root.transform, "Partition_PantryBlock", pantryCenterX, zBackCenter, pantryW, backDepth, openingSide: RectOpeningSide.West, openingWidth: 2f);

        // Walls for ServerRoom (a small partition within back-right area)
        CreateRectangularPartition(root.transform, "Partition_ServerRoom", serverPos.x, serverPos.z, serverW, serverD, openingSide: RectOpeningSide.East, openingWidth: 1.5f);

        // Walls for Reception & Storage (front row partitions)
        CreateRectangularPartition(root.transform, "Partition_Reception", receptionCenterX, zFrontCenter, frontSegW, frontDepth, openingSide: RectOpeningSide.North, openingWidth: 2.2f);
        CreateRectangularPartition(root.transform, "Partition_StorageRoom", storageCenterX, zFrontCenter, frontSegW, frontDepth, openingSide: RectOpeningSide.North, openingWidth: 1.6f);

        // Minor partition between open office and back row near center to match reference small divider
        CreateWall(root.transform, "Wall_Center_Left", new Vector3(-(openWidth * 0.25f), wallY, wallZBetweenBackAndOpen), new Vector3(6f, wallHeight, wallThickness));
        CreateWall(root.transform, "Wall_Center_Right", new Vector3((openWidth * 0.25f), wallY, wallZBetweenBackAndOpen), new Vector3(6f, wallHeight, wallThickness));

        Undo.CollapseUndoOperations(g);

        // Run validation to detect overlaps / out-of-bounds
        ValidateLayout(root);

        Debug.Log("Mission_01_FromImage generated. Objects under root: " + rootName + ". Use Clear menu to remove.");
    }

    [MenuItem("Tools/Network Shift/Clear Office From Image")]
    public static void Clear()
    {
        var root = GameObject.Find(rootName);
        if (root == null)
        {
            EditorUtility.DisplayDialog("Clear Office From Image", "No Mission_01_FromImage found in the scene.", "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog("Clear Office From Image", $"Remove '{rootName}' and all generated geometry?", "Yes", "No"))
            return;

        Undo.DestroyObjectImmediate(root);
        Debug.Log("Removed " + rootName);
    }

    // ---------- Helpers ----------

    static void CreateFloorOrCeiling(Transform parent, string name, float width, float thickness, float depth, Vector3 center)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        go.transform.localScale = new Vector3(width, thickness, depth);
        go.transform.localPosition = center;
    }

    static void CreateFloor(Transform parent, string name, float width, float depth, Vector3 center)
    {
        CreateFloorOrCeiling(parent, name, width, floorThickness, depth, center);
    }

    static void CreateWall(Transform parent, string name, Vector3 center, Vector3 size)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = center;
        go.transform.localScale = size;
    }

    // Create a long wall across the building but leave a centered rectangular opening of openingWidth at openingCenterX
    static void CreateWallWithOpening(Transform parent, string baseName, Vector3 center, float totalLengthX, float openingCenterX, float openingWidth)
    {
        float leftMost = -(totalLengthX * 0.5f);
        float rightMost = (totalLengthX * 0.5f);

        // Left segment
        float leftSegWidth = (openingCenterX - (openingWidth * 0.5f)) - leftMost;
        if (leftSegWidth > 0.01f)
        {
            float leftCenter = leftMost + leftSegWidth * 0.5f;
            Vector3 segCenter = new Vector3(center.x + leftCenter, center.y, center.z);
            CreateWall(parent, baseName + "_Left", segCenter, new Vector3(leftSegWidth, wallHeight, wallThickness));
        }

        // Right segment
        float rightSegWidth = rightMost - (openingCenterX + (openingWidth * 0.5f));
        if (rightSegWidth > 0.01f)
        {
            float rightCenter = (openingCenterX + (openingWidth * 0.5f)) + rightSegWidth * 0.5f;
            Vector3 segCenter = new Vector3(center.x + rightCenter, center.y, center.z);
            CreateWall(parent, baseName + "_Right", segCenter, new Vector3(rightSegWidth, wallHeight, wallThickness));
        }
    }

    enum RectOpeningSide { North, South, East, West }

    // Create walls around a rectangular room (centerX/centerZ are center coords), leaving a single opening on openingSide with openingWidth.
    // This is used to create room partitions with a doorway.
    static void CreateRectangularPartition(Transform parent, string baseName, float centerX, float centerZ, float sizeX, float sizeZ, RectOpeningSide openingSide, float openingWidth)
    {
        float halfX = sizeX * 0.5f;
        float halfZ = sizeZ * 0.5f;
        float yCenter = wallHeight * 0.5f;

        // North wall (+Z)
        CreateWall(parent, baseName + "_North", new Vector3(centerX, yCenter, centerZ + halfZ + wallThickness * 0.5f), new Vector3(sizeX + 0.05f, wallHeight, wallThickness));
        // South wall (-Z)
        CreateWall(parent, baseName + "_South", new Vector3(centerX, yCenter, centerZ - halfZ - wallThickness * 0.5f), new Vector3(sizeX + 0.05f, wallHeight, wallThickness));
        // West/East handle opening by splitting segments
        if (openingSide == RectOpeningSide.West)
        {
            CreateWallWithOpening_Side(parent, baseName + "_West", new Vector3(centerX - halfX - wallThickness * 0.5f, yCenter, centerZ), sizeZ, openingWidth, true);
        }
        else
        {
            CreateWall(parent, baseName + "_West", new Vector3(centerX - halfX - wallThickness * 0.5f, yCenter, centerZ), new Vector3(wallThickness, wallHeight, sizeZ + 0.05f));
        }

        if (openingSide == RectOpeningSide.East)
        {
            CreateWallWithOpening_Side(parent, baseName + "_East", new Vector3(centerX + halfX + wallThickness * 0.5f, yCenter, centerZ), sizeZ, openingWidth, true);
        }
        else
        {
            CreateWall(parent, baseName + "_East", new Vector3(centerX + halfX + wallThickness * 0.5f, yCenter, centerZ), new Vector3(wallThickness, wallHeight, sizeZ + 0.05f));
        }
    }

    // Create a vertical wall (along Z) with centered opening of width openingWidth (split into top/bottom segments)
    static void CreateWallWithOpening_Side(Transform parent, string baseName, Vector3 wallCenter, float fullLengthZ, float openingWidth, bool vertical)
    {
        float leftover = fullLengthZ - openingWidth;
        if (leftover <= 0.01f) return;
        float halfLeft = leftover * 0.5f;

        float topCenterZ = wallCenter.z + (openingWidth * 0.5f) + (halfLeft * 0.5f);
        float bottomCenterZ = wallCenter.z - (openingWidth * 0.5f) - (halfLeft * 0.5f);

        // top segment
        CreateWall(parent, baseName + "_Top", new Vector3(wallCenter.x, wallCenter.y, topCenterZ), new Vector3(wallThickness, wallHeight, halfLeft));
        // bottom segment
        CreateWall(parent, baseName + "_Bottom", new Vector3(wallCenter.x, wallCenter.y, bottomCenterZ), new Vector3(wallThickness, wallHeight, halfLeft));
    }

    // Validation: checks for overlapping floors/walls and out-of-bounds floors
    static void ValidateLayout(GameObject root)
    {
        var all = root.transform.Cast<Transform>().Select(t => t.gameObject).ToList();
        var floors = all.Where(g => g.name.StartsWith("Floor_")).ToList();
        var walls = all.Where(g => g.name.StartsWith("Wall_") || g.name.StartsWith("Partition_")).ToList();

        Bounds GetBounds(GameObject g)
        {
            var r = g.GetComponent<Renderer>();
            if (r != null) return r.bounds;
            Vector3 center = g.transform.position;
            Vector3 size = Vector3.Scale(g.transform.localScale, g.transform.lossyScale);
            return new Bounds(center, size);
        }

        bool problems = false;

        // Floor-floor overlaps
        for (int i = 0; i < floors.Count; i++)
        {
            for (int j = i + 1; j < floors.Count; j++)
            {
                var a = GetBounds(floors[i]);
                var b = GetBounds(floors[j]);
                if (a.Intersects(b))
                {
                    float vol = IntersectionVolume(a, b);
                    if (vol > 0.0001f)
                    {
                        Debug.LogError($"[Validation] Floor overlap: '{floors[i].name}' vs '{floors[j].name}', vol {vol:F4}", floors[i]);
                        problems = true;
                    }
                }
            }
        }

        // Wall-wall overlaps (non-essential but warn)
        for (int i = 0; i < walls.Count; i++)
        {
            for (int j = i + 1; j < walls.Count; j++)
            {
                var a = GetBounds(walls[i]);
                var b = GetBounds(walls[j]);
                if (a.Intersects(b))
                {
                    float vol = IntersectionVolume(a, b);
                    if (vol > 0.01f)
                    {
                        Debug.LogWarning($"[Validation] Wall overlap: '{walls[i].name}' vs '{walls[j].name}', vol {vol:F3}", walls[i]);
                        problems = true;
                    }
                }
            }
        }

        // Floors inside footprint
        Bounds footprint = new Bounds(Vector3.zero, new Vector3(buildingWidth, 20f, buildingDepth));
        foreach (var f in floors)
        {
            var b = GetBounds(f);
            if (!footprint.Contains(b.min) || !footprint.Contains(b.max))
            {
                Debug.LogWarning($"[Validation] Floor '{f.name}' lies partially outside the building footprint.", f);
                problems = true;
            }
        }

        if (!problems) Debug.Log("Validation passed: no major overlaps or out-of-bounds floors detected.");
        else Debug.Log("Validation completed with issues; check Console messages (click to highlight objects).");
    }

    static float IntersectionVolume(Bounds a, Bounds b)
    {
        Vector3 min = Vector3.Max(a.min, b.min);
        Vector3 max = Vector3.Min(a.max, b.max);
        Vector3 diff = max - min;
        if (diff.x <= 0f || diff.y <= 0f || diff.z <= 0f) return 0f;
        return diff.x * diff.y * diff.z;
    }
}