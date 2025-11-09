# Système UI - Escape Facility

## Vue d'ensemble

Le système UI du jeu comprend plusieurs composants qui travaillent ensemble pour fournir une interface utilisateur complète et immersive.

## Composants UI

### 1. DetectionUI (Barre de détection)
**Fichier**: `Assets/Scripts/UI/DetectionUI.cs`

**Fonctionnalités**:
- Barre de détection qui se remplit progressivement quand un garde voit le joueur
- Transition de couleur de jaune (détection faible) à rouge (détection complète)
- Décroissance progressive de la barre quand le joueur échappe à la détection
- Écran rouge "CAUGHT!" avec effet de flash lorsque le joueur est capturé
- Pause automatique du jeu lors de la capture

**Configuration dans Unity**:
1. Créer un Panel UI pour la barre de détection
2. Ajouter une Image avec `Image Type: Filled` et `Fill Method: Horizontal`
3. Créer un Panel pour l'écran "Caught!" avec un fond rouge semi-transparent
4. Ajouter un Text "CAUGHT!" au centre
5. Assigner les références dans l'inspecteur
6. Tagger tous les gardes avec le tag "Guard"

**Paramètres ajustables**:
- `detectionSpeed`: Vitesse de remplissage de la barre (défaut: 0.5)
- `detectionDecaySpeed`: Vitesse de décroissance de la barre (défaut: 1.0)
- `detectionColorStart`: Couleur de départ (défaut: jaune)
- `detectionColorEnd`: Couleur de fin (défaut: rouge)
- `caughtFlashDuration`: Durée de l'effet flash (défaut: 2 secondes)

**Méthodes publiques**:
```csharp
public void ResetDetection()  // Réinitialise la détection (utile pour respawn)
public void SetCaught()        // Force l'état capturé
public bool IsCaught    // Vérifie si le joueur est capturé
public float DetectionLevel    // Niveau de détection actuel (0-1)
```

---

### 2. MiniMapCamera (Mini-carte)
**Fichier**: `Assets/Scripts/UI/MiniMapCamera.cs`

**Fonctionnalités**:
- Caméra orthographique suivant le joueur
- Vue de dessus de la scène
- Zoom ajustable
- Mode suivi activable/désactivable

**Configuration dans Unity**:
1. Créer un nouveau GameObject "MiniMapCamera"
2. Ajouter le script `MiniMapCamera`
3. Créer une RawImage dans le Canvas pour afficher la mini-map
4. La caméra trouvera automatiquement le joueur avec le tag "Player"

**Paramètres ajustables**:
- `height`: Hauteur de la caméra au-dessus du joueur (défaut: 20)
- `cameraSize`: Taille orthographique / niveau de zoom (défaut: 10)
- `followPlayer`: Active/désactive le suivi (défaut: true)

**Méthodes publiques**:
```csharp
public void SetZoom(float size)  // Ajuste le niveau de zoom
public void SetFollowPlayer(bool follow) // Active/désactive le suivi
```

---

### 3. SecurityCamera (Caméras de surveillance)
**Fichier**: `Assets/Scripts/GameUtils/SecurityCamera.cs`

**Fonctionnalités**:
- Rotation automatique dans un arc défini
- Détection du joueur dans un cône de vision
- Feedback visuel (changement de couleur de lumière)
- Activation/désactivation individuelle
- Intégration avec le système d'alerte

**Configuration dans Unity**:
1. Créer un GameObject pour la caméra
2. Ajouter une lumière (Light) pour le feedback visuel
3. Ajouter le script `SecurityCamera`
4. Tagger le joueur avec "Player"
5. Configurer les paramètres de rotation et détection

**Paramètres ajustables**:
- `cameraName`: Nom de la caméra
- `canRotate`: Active la rotation (défaut: true)
- `rotationSpeed`: Vitesse de rotation (défaut: 20)
- `rotationAngle`: Angle d'oscillation (défaut: 60°)
- `detectionRange`: Portée de détection (défaut: 15m)
- `detectionFOV`: Champ de vision (défaut: 90°)
- `detectionColor`: Couleur normale (défaut: vert)
- `alertColor`: Couleur d'alerte (défaut: rouge)

**Méthodes publiques**:
```csharp
public void ToggleCamera()    // Inverse l'état actif/inactif
public void SetCameraActive(bool active) // Définit l'état de la caméra
public string CameraName         // Nom de la caméra
public bool IsActive   // État actif
public Camera CameraComponent            // Composant caméra
```

---

### 4. CameraControlUI (Interface de contrôle des caméras)
**Fichier**: `Assets/Scripts/UI/CameraControlUI.cs`

**Fonctionnalités**:
- Panel de contrôle des caméras de surveillance
- Boutons pour activer/désactiver chaque caméra
- Affichage de la vue de la caméra sélectionnée
- Basculement avec la touche Tab
- Sélection rapide avec les touches numériques (1-9)

**Configuration dans Unity**:
1. Créer un Panel UI pour le contrôle des caméras
2. Créer un container (Vertical/Horizontal Layout Group) pour les boutons
3. Créer un prefab de bouton avec un Text
4. Créer une RawImage pour l'affichage de la vue caméra
5. Assigner les références dans l'inspecteur

**Contrôles**:
- `Tab`: Ouvre/ferme le panel de contrôle
- `1-9`: Sélectionne la caméra correspondante
- `Clic sur bouton`: Active/désactive la caméra

---

### 5. GameUIManager (Gestionnaire principal UI)
**Fichier**: `Assets/Scripts/UI/GameUIManager.cs`

**Fonctionnalités**:
- Gestionnaire centralisé de tous les composants UI
- Affichage des objectifs
- Affichage du statut (Safe/Alert/Caught)
- Coordination entre les différents systèmes UI
- Singleton pour accès global

**Configuration dans Unity**:
1. Créer un GameObject "GameUIManager"
2. Ajouter le script `GameUIManager`
3. Les composants UI seront trouvés automatiquement ou peuvent être assignés manuellement
4. Créer des Text UI pour objectifs et statut

**Méthodes publiques**:
```csharp
public void UpdateObjective(string objective)          // Met à jour l'objectif affiché
public void ShowNotification(string message, float duration) // Affiche une notification
public void SetHUDVisible(bool visible)                // Affiche/cache le HUD
public DetectionUI GetDetectionUI()     // Accède au système de détection
public CameraControlUI GetCameraControlUI()            // Accède au contrôle caméras
public MiniMapCamera GetMiniMapCamera()       // Accède à la mini-map
```

**Accès au Singleton**:
```csharp
GameUIManager.Instance.UpdateObjective("Trouvez la sortie");
```

---

### 6. PauseMenu (Menu pause)
**Fichier**: `Assets/Scripts/UI/PauseMenu.cs`

**Fonctionnalités**:
- Menu pause avec touche Échap
- Pause/reprise du jeu via Time.timeScale
- Bouton de reprise

**Configuration dans Unity**:
1. Créer un Panel UI pour le menu pause
2. Ajouter un bouton "Resume"
3. Assigner les références dans l'inspecteur

**Contrôles**:
- `Échap`: Ouvre/ferme le menu pause

---

## Hiérarchie UI recommandée

```
Canvas
??? HUD
?   ??? DetectionBar (Panel)
?   ?   ??? FillImage (Image avec Fill Amount)
?   ??? MiniMap (RawImage)
?   ??? ObjectiveText (Text)
?   ??? StatusText (Text)
??? CaughtScreen (Panel)
?   ??? BackgroundImage (Image rouge semi-transparent)
?   ??? CaughtText (Text)
??? CameraControlPanel (Panel)
?   ??? CameraDisplay (RawImage)
?   ??? CameraNameText (Text)
?   ??? ButtonContainer (Vertical Layout Group)
?       ??? CameraButtonPrefab (Button avec Text)
??? PauseMenu (Panel)
    ??? ResumeButton (Button)

Scene
??? GameUIManager
??? MiniMapCamera
??? SecurityCameras
    ??? Camera1
    ??? Camera2
    ??? Camera3
```

---

## Tags requis

Assurez-vous que les tags suivants sont définis dans Unity:
- **Player**: À assigner au GameObject du joueur
- **Guard**: À assigner à tous les gardes

---

## Intégration avec les autres systèmes

### Avec GuardAI
Le système DetectionUI détecte automatiquement les gardes et vérifie leur ligne de vue vers le joueur.

### Avec SecurityCamera
Les caméras peuvent déclencher des alertes qui peuvent être intégrées avec le AIManager pour alerter les gardes.

### Exemple d'utilisation
```csharp
// Dans un script de gameplay
if (GameUIManager.Instance != null)
{
    GameUIManager.Instance.UpdateObjective("Atteindre la porte nord");
    
    if (GameUIManager.Instance.GetDetectionUI().IsCaught)
    {
     // Logique de game over
  }
}
```

---

## Notes de développement

- Les scripts utilisent `UnityEngine.UI.Text` au lieu de `TextMeshPro` pour éviter les dépendances externes
- Pour de meilleures performances de texte, vous pouvez remplacer `Text` par `TextMeshProUGUI` si vous avez le package TextMesh Pro installé
- Le système de détection utilise des raycasts, considérez l'optimisation pour de nombreux gardes
- Les render textures des caméras peuvent impacter les performances, ajustez leur résolution selon les besoins

---

## TODO / Améliorations futures

- [ ] Système de notifications temporaires
- [ ] Mini-map avec icônes pour joueur/gardes
- [ ] Enregistrement de replays des caméras
- [ ] Interface de hacking pour désactiver temporairement les caméras
- [ ] Indicateur de niveau sonore
- [ ] Barre de santé/stamina
- [ ] Système d'inventaire UI
