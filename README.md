# File Name Replace (com.actionfit.filenamereplace)

선택한 폴더 안의 파일 이름을 **일괄 변경**하는 Unity 에디터 툴입니다. 문자열 치환·접두/접미사 추가·키워드 포함 파일 삭제를 **미리보기 후 실행**합니다. 게임 고유 타입과 컴파일 결합이 없습니다.

## 설치 (manifest.json, Git URL)

```json
{
  "dependencies": {
    "com.actionfit.filenamereplace": "https://github.com/ActionFit-Editor/File_Name_Replace.git#1.0.6"
  }
}
```

비공개 repo면 각 사용자의 Git이 해당 org repo에 인증돼 있어야 합니다(HTTPS는 PAT/Credential Manager, SSH는 키 등록).

## Agent Skill 안내

패키지를 설치하거나 업데이트한 뒤 `Tools > Package > Custom Package Manager > Install or Refresh Agent Skills`를 실행합니다.

- `$file-rename-help`: target 선택, rename mode, 삭제 preview, 메뉴와 파괴적 작업 경계를 설명합니다.

이 패키지는 의도적으로 help만 등록합니다. Skill은 파일 rename/delete/move, `.meta` 편집, Project 선택 변경 또는 asset 참조 변경을 실행하지 않습니다.

## Unity 메뉴

- Package root: `Tools > Package > File Name Replace`.
- README: `Tools > Package > File Name Replace > README`.
- 패키지 명령은 같은 package root 아래에 유지하며 README/Setting SO 항목이 있으면 분리된 해당 항목보다 위에 표시합니다.

## 구성

- **Editor** (`com.actionfit.filenamereplace.Editor`): `FileNameReplaceTool` 에디터 윈도우.

## 사용

1. `Tools > Package > File Name Replace > Open Window` 로 윈도우 열기.
2. **Target Folder** 에 대상 폴더(DefaultAsset)를 지정.
3. 작업 선택:
   - **Replace (Before → After)**: 파일명의 `String Before`를 `String After`로 치환.
   - **Add Prefix / Add Suffix**: 파일명 앞/뒤에 문자열 추가.
   - **Delete Files (키워드 포함)**: `Delete Keyword`를 포함한 파일 삭제.
4. 실행 시 대상 목록 미리보기 확인 후 적용됩니다.
