MSBUILD=/usr/bin/xbuild
MODNAME=Lottery

all: windows mono

mono:
	$(MSBUILD)
	cp bin/Mono/$(MODNAME).dll Mono.dll

windows:
	$(MSBUILD) /property:Configuration=Windows
	cp bin/Windows/$(MODNAME).dll Windows.dll

clean:
	rm -rf obj/ bin/
