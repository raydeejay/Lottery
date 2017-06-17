MSBUILD=/usr/bin/xbuild
MODNAME=Lottery
TERRARIA=${HOME}/.local/share/Steam/steamapps/common/Terraria/Terraria

all: windows mono
	${TERRARIA} -build `pwd`
mono:
	$(MSBUILD)
	cp bin/Mono/$(MODNAME).dll Mono.dll

windows:
	$(MSBUILD) /property:Configuration=Windows
	cp bin/Windows/$(MODNAME).dll Windows.dll

clean:
	rm -rf obj/ bin/
