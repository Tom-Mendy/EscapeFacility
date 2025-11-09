# Guide de mise en place rapide - Système UI
## Configuration en 5 minutes

### ?? Objectif
Mettre en place tous les éléments UI du jeu Escape Facility rapidement et facilement.

---

## ?? Méthode 1: Configuration automatique (RECOMMANDÉ)

### Étapes:

1. **Créer les tags nécessaires**
   - Aller dans `Edit ? Project Settings ? Tags and Layers`
   - Ajouter les tags:
     - `Player`
     - `Guard`

2. **Créer la structure de base**
   ```
   Hiérarchie:
- Canvas (si pas déjà présent)
   - Player (avec tag "Player")
   - Guards (plusieurs GameObjects avec tag "Guard")
   ```

3. **Ajouter le GameUIManager**
   - Créer un GameObject vide nommé "GameUIManager"
   - Ajouter le script `GameUIManager.cs`
   - Laisser tous les champs vides (détection automatique)

4. **Utiliser l'assistant de setup**
   - Créer un GameObject nommé "UISetupHelper"
   - Ajouter le script `UISetupHelper.cs`
   - Dans l'inspecteur du UISetupHelper:
     - Assigner le Canvas dans le champ "Main Canvas"
     - Clic droit sur le script ? "Find and Assign References"
     - Clic droit sur le script ? "Setup UI Components"
   - ? Toute l'interface est créée automatiquement!

5. **Ajouter le DetectionUI script**
   - Trouver le GameObject "GameUIManager"
   - Ajouter le composant `DetectionUI.cs`
   - Assigner les références depuis la hiérarchie:
     - `Detection Bar Panel` ? DetectionBarPanel
     - `Detection Fill Image` ? DetectionBarPanel/FillImage
  - `Caught Panel` ? CaughtPanel
   - `Caught Screen Image` ? CaughtPanel (Image component)
     - `Caught Text` ? CaughtPanel/CaughtText
   - Laisser le champ "Guards" vide (détection automatique par tag)

6. **Tester**
   - Lancer le jeu
   - Approchez-vous d'un garde
   - La barre de détection devrait apparaître!

---

## ?? Méthode 2: Configuration manuelle complète

### A. Créer la barre de détection

1. Dans le Canvas, créer:
   ```
   DetectionBarPanel (Panel)
   ??? FillImage (Image)
   ```

2. Configuration DetectionBarPanel:
   - Anchor: Center Top
   - Position: (0, -50)
   - Taille: (400, 30)
   - Couleur fond: Noir semi-transparent (0, 0, 0, 128)

3. Configuration FillImage:
   - Stretch (ancres aux 4 coins)
   - Marges: 5 pixels de chaque côté
   - Image Type: **Filled**
   - Fill Method: **Horizontal**
- Fill Origin: **Left**
   - Fill Amount: **0**
   - Couleur: Jaune

### B. Créer l'écran "Caught!"

1. Dans le Canvas, créer:
   ```
   CaughtPanel (Panel)
   ??? CaughtText (Text)
   ```

2. Configuration CaughtPanel:
   - Stretch (ancres aux 4 coins)
   - Offsets: tous à 0
   - Couleur: Rouge semi-transparent (255, 0, 0, 128)

3. Configuration CaughtText:
   - Anchor: Center
   - Position: (0, 0)
   - Taille: (400, 100)
   - Texte: "CAUGHT!"
   - Font Size: 72
   - Alignement: Center
   - Couleur: Blanc

### C. Créer la mini-map

1. Créer un nouveau GameObject nommé "MiniMapCamera"
2. Ajouter une Camera:
   - Projection: Orthographic
   - Size: 10
   - Clear Flags: Solid Color
   - Background: Noir

3. Ajouter le script `MiniMapCamera.cs`

4. Dans le Canvas, créer:
   ```
   MiniMapDisplay (RawImage)
   ```

5. Configuration MiniMapDisplay:
   - Anchor: Top Right
   - Pivot: (1, 1)
   - Position: (-10, -10)
   - Taille: (200, 200)

6. Créer une RenderTexture:
   - Assets ? Create ? Render Texture
   - Nom: "MiniMapRT"
   - Taille: 256x256
   - Assigner à MiniMapCamera ? Target Texture
   - Assigner à MiniMapDisplay ? Texture

### D. Créer les caméras de surveillance

1. Pour chaque caméra de surveillance:
   - Créer un GameObject "SecurityCamera_1"
   - Ajouter une Camera
   - Ajouter une Light (Spot light)
   - Ajouter le script `SecurityCamera.cs`

2. Configuration SecurityCamera.cs:
   - Camera Name: "Camera 1"
   - Can Rotate: ?
 - Rotation Speed: 20
   - Rotation Angle: 60
   - Detection Range: 15
   - Detection FOV: 90
   - Detection Layers: Everything (ou spécifique)

3. Positionner les caméras dans la scène

### E. Créer le panneau de contrôle caméras

1. Dans le Canvas, créer:
   ```
   CameraControlPanel (Panel)
   ??? CameraDisplay (RawImage)
   ??? CameraNameText (Text)
   ??? ButtonContainer (GameObject)
   ```

2. Configuration CameraControlPanel:
   - Anchor: Left Center
   - Pivot: (0, 0.5)
   - Position: (10, 0)
   - Taille: (250, 400)
   - Couleur fond: Gris foncé (26, 26, 26, 230)

3. Configuration CameraDisplay:
   - Parent: CameraControlPanel
   - Anchor: Top Center
   - Position: (0, -10)
   - Taille: (230, 150)

4. Configuration CameraNameText:
   - Parent: CameraControlPanel
   - Anchor: Top Center
   - Position: (0, -170)
   - Taille: (230, 30)
   - Texte: "No Camera Selected"

5. Configuration ButtonContainer:
   - Ajouter Vertical Layout Group:
     - Spacing: 5
     - Child Force Expand: Width only
   - Anchor: Top Center
   - Position: (0, -210)

6. Créer un prefab de bouton:
   - Créer un Button
   - Taille: (230, 40)
   - Ajouter un Text enfant
   - Sauvegarder comme Prefab
   - Assigner à CameraControlUI ? Camera Button Prefab

### F. Finaliser

1. Créer GameObject "GameUIManager"
2. Ajouter `GameUIManager.cs`
3. Assigner toutes les références
4. Ajouter GameObject avec `DetectionUI.cs`
5. Ajouter GameObject avec `CameraControlUI.cs`

---

## ? Checklist de vérification

Avant de lancer le jeu, vérifier:

- [ ] Les tags "Player" et "Guard" existent
- [ ] Le joueur a le tag "Player"
- [ ] Tous les gardes ont le tag "Guard"
- [ ] GameUIManager est dans la scène
- [ ] DetectionUI a toutes ses références assignées
- [ ] CameraControlUI a le button prefab assigné
- [ ] MiniMapCamera a sa RenderTexture
- [ ] Toutes les caméras de surveillance ont le script SecurityCamera
- [ ] Le Canvas est configuré (Screen Space - Overlay)

---

## ?? Test de fonctionnement

### Test 1: Barre de détection
1. Lancer le jeu
2. Approcher un garde (tag "Guard")
3. Se placer devant lui (dans son champ de vision)
4. **Résultat attendu**: La barre jaune apparaît et se remplit
5. S'éloigner
6. **Résultat attendu**: La barre décroît et disparaît

### Test 2: Écran "Caught!"
1. Lancer le jeu
2. Rester devant un garde jusqu'à ce que la barre soit pleine
3. **Résultat attendu**: 
   - Écran rouge s'affiche
   - Texte "CAUGHT!" visible
   - Effet de pulsation
   - Jeu en pause

### Test 3: Mini-map
1. Lancer le jeu
2. Regarder en haut à droite
3. **Résultat attendu**: 
   - Vue de dessus visible
   - Suit le joueur quand il bouge

### Test 4: Caméras de surveillance
1. Lancer le jeu
2. Observer une caméra de surveillance
3. **Résultat attendu**: 
   - La caméra pivote
   - La lumière est verte
4. Se placer devant la caméra
5. **Résultat attendu**: La lumière devient rouge

### Test 5: Contrôle des caméras
1. Lancer le jeu
2. Appuyer sur Tab
3. **Résultat attendu**: 
   - Panneau de contrôle apparaît
   - Liste des caméras visible
   - Curseur visible
4. Cliquer sur un bouton
5. **Résultat attendu**: État de la caméra change (ON/OFF)

---

## ?? Problèmes courants et solutions

### Problème: "Tag 'Player' not found"
**Solution**: Créer le tag dans Edit ? Project Settings ? Tags and Layers

### Problème: La barre ne se remplit pas
**Solutions possibles**:
1. Vérifier que FillImage ? Fill Amount commence à 0
2. Vérifier que FillImage ? Image Type est "Filled"
3. Vérifier que les gardes ont le tag "Guard"
4. Vérifier la distance entre joueur et garde (< 15m par défaut)

### Problème: Mini-map noire
**Solutions possibles**:
1. Vérifier que la RenderTexture est assignée à la caméra
2. Vérifier que la RenderTexture est assignée à la RawImage
3. Vérifier le Culling Mask de MiniMapCamera
4. Augmenter la hauteur de la caméra

### Problème: Curseur reste bloqué
**Solution**: Le script CameraControlUI gère le curseur. Vérifier que le panneau se ferme bien avec Tab.

### Problème: Game ne se débloque pas après "Caught!"
**Solution**: Appeler `detectionUI.ResetDetection()` depuis un bouton restart/menu

---

## ?? Astuces d'optimisation

1. **RenderTextures**: Garder une résolution basse (256x256) pour les performances
2. **Nombre de caméras**: Limiter à 3-5 caméras actives simultanément
3. **Fréquence de détection**: Le système check chaque frame, c'est acceptable pour ~10 gardes
4. **Layers**: Utiliser des layers pour filtrer les raycasts (ignore environment, etc.)

---

## ?? Ressources supplémentaires

- **Documentation complète**: `Docs/UI_System_README.md`
- **Résumé implémentation**: `Docs/UI_Implementation_Summary_FR.md`
- **Scripts**: `Assets/Scripts/UI/` et `Assets/Scripts/GameUtils/`

---

## ?? C'est terminé!

Votre système UI est maintenant configuré et prêt à l'emploi.

**Bon développement!** ??
